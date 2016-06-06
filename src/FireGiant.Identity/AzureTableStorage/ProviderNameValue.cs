// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireGiant.Identity.AzureTableStorage
{
    class ProviderNameValue
    {
        public string Provider { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public static IEnumerable<ProviderNameValue> Parse(string tokens)
        {
            if (tokens == null)
            {
                yield break;
            }

            var start = 0;
            var end = -1;

            while ((end = tokens.IndexOf(Constants.NameValueSeparator, start, StringComparison.Ordinal)) > 0)
            {
                var provider = tokens.Substring(start, end - start);

                start = end + 1;
                end = tokens.IndexOf(Constants.NameValueSeparator, start, StringComparison.Ordinal);

                var name = tokens.Substring(start, end - start);

                start = end + 1;
                end = tokens.IndexOf(Constants.NameValuePairSeparator, start, StringComparison.Ordinal);

                var value = tokens.Substring(start, end - start);

                if (value.StartsWith(Constants.ValueEncodedPrefix, StringComparison.Ordinal))
                {
                    value = value.Substring(Constants.ValueEncodedPrefix.Length).FromBase64();
                }

                yield return new ProviderNameValue { Provider = provider, Name = name, Value = value };

                start = end + 1;
            }
        }

        public static string Generate(IEnumerable<ProviderNameValue> tokens)
        {
            if (!tokens.Any())
            {
                return null;
            }

            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var value = token.Value ?? String.Empty;

                if (value.StartsWith(Constants.ValueEncodedPrefix, StringComparison.Ordinal) || value.Contains(Constants.NameValuePairSeparator))
                {
                    value = Constants.ValueEncodedPrefix + value.ToBase64();
                }

                result.Append(token.Provider);
                result.Append(Constants.NameValuePairSeparator);
                result.Append(token.Name);
                result.Append(Constants.NameValueSeparator);
                result.Append(value);
                result.Append(Constants.NameValuePairSeparator);
            }

            return result.ToString();
        }
    }
}