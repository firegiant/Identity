// Copyright (c) FireGiant.  All Rights Reserved.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserAuthenticationTokenStore<TUser>
    {
        public Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var tokens = ProviderNameValue.Parse(user.Tokens);

            var value = tokens.Where(t => t.Provider == loginProvider && t.Name == name).Select(t => t.Value).FirstOrDefault();

            return Task.FromResult(value);
        }

        public Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var tokens = ProviderNameValue.Parse(user.Tokens).ToList();

            if (0 < tokens.RemoveAll(t => t.Provider == loginProvider && t.Name == name))
            {
                user.Tokens = ProviderNameValue.Generate(tokens);
            }

            return Task.FromResult(0);
        }

        public Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            var tokens = ProviderNameValue.Parse(user.Tokens).ToList();

            tokens.RemoveAll(t => t.Provider == loginProvider && t.Name == name);

            tokens.Add(new ProviderNameValue { Provider = loginProvider, Name = name, Value = value });

            user.Tokens = ProviderNameValue.Generate(tokens);

            return Task.FromResult(0);
        }
    }
}