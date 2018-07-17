// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireGiant.Identity.AzureTableStorage;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<TUser> FindByNameOrEmailAsync<TUser>(this UserManager<TUser> userManager, string nameOrEmail) where TUser : class
        {
            return await userManager.FindByNameAsync(nameOrEmail) ?? await userManager.FindByEmailAsync(nameOrEmail);
        }

        public static async Task AddAccess<TUser>(this UserManager<TUser> userManager, TUser user, string ip, string userAgent, string result) where TUser : AzureUser
        {
            if (userManager is AzureUserManager<TUser> aum)
            {
                await aum.AddAccess(user, ip, userAgent, result);
            }
        }

        public static async Task<IEnumerable<UserAccess>> GetAccesses<TUser>(this UserManager<TUser> userManager, TUser user) where TUser : AzureUser
        {
            if (userManager is AzureUserManager<TUser> aum)
            {
                return await aum.GetAccesses(user);
            }

            return Enumerable.Empty<UserAccess>();
        }
    }
}