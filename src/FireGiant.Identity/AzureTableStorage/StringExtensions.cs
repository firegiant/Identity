// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace FireGiant.Identity.AzureTableStorage
{
    internal static class StringExtensions
    {
        private static readonly Regex UnsafeCharacters = new Regex(@"[^a-zA-Z\d~!@$%^&*()\-_=+\[\]{};:'"",<\.> \t]+", RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        public static string ToBase64(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public static string ToBase64IfUnsafe(this string value)
        {
            return UnsafeCharacters.IsMatch(value) ? value.ToBase64() : value;
        }

        public static string FromBase64(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            value = value.Replace('-', '+').Replace('_', '/');

            var mod = value.Length % 4;
            if (mod > 0)
            {
                value += new string('=', 4 - mod);
            }

            var bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}