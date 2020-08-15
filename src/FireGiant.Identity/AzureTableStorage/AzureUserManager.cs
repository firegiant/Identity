// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FireGiant.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FireGiant.Identity.AzureTableStorage
{
    public class AzureUserManager<TUser> : UserManager<TUser> where TUser : AzureUser
    {
        public AzureUserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task AddAccess(TUser user, string ip, string userAgent, string result)
        {
            if (this.Store is AzureUserStore<TUser> store)
            {
                var access = new UserAccess
                {
                    IP = ip,
                    Reason = result,
                    UserAgent = userAgent,
                    When = DateTimeOffset.UtcNow
                };

                await store.AddAccess(user, access);
            }
        }

        public async Task<IEnumerable<UserAccess>> GetAccesses(TUser user)
        {
            if (this.Store is AzureUserStore<TUser> store)
            {
                return await store.GetAccess(user);
            }

            return Array.Empty<UserAccess>();
        }
    }
}
