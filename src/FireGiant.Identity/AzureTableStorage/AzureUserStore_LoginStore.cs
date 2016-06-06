// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserLoginStore<TUser>
    {
        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            var existing = ProviderNameValue.Parse(user.Logins).ToList();

            existing.Add(new ProviderNameValue { Provider = login.LoginProvider, Name = login.ProviderKey, Value = login.ProviderDisplayName });

            user.Logins = ProviderNameValue.Generate(existing);

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            var logins = ProviderNameValue.Parse(user.Logins)
                .Select(l => new UserLoginInfo(l.Provider, l.Name, l.Value))
                .ToList();

            return Task.FromResult((IList<UserLoginInfo>)logins);
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var existing = ProviderNameValue.Parse(user.Logins).ToList();

            if (0 < existing.RemoveAll(l => l.Provider == loginProvider && l.Name == providerKey))
            {
                user.Logins = ProviderNameValue.Generate(existing);
            }

            return Task.FromResult(0);
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}