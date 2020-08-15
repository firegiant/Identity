// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FireGiant.Identity.AzureTableStorage;
using FireGiant.Identity.TokenProviders;
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

            return Array.Empty<UserAccess>();
        }

        public static Task<string> GeneratePasswordlessAccessTokenAsync<TUser>(this UserManager<TUser> userManager, TUser user) where TUser : class
        {
            return userManager.GenerateUserTokenAsync(user, FireGiantTokenProviderConstants.PasswordlessLoginTokenProvider, FireGiantTokenProviderConstants.PasswordlessLoginPurpose);
        }

        public static Task<string> GeneratePasswordlessShortAccessTokenAsync<TUser>(this UserManager<TUser> userManager, TUser user) where TUser : class
        {
            return userManager.GenerateUserTokenAsync(user, FireGiantTokenProviderConstants.PasswordlessLoginTotpTokenProvider, FireGiantTokenProviderConstants.PasswordlessLoginPurpose);
        }

        public static Task<bool> VerifyPasswordlessAccessTokenAsync<TUser>(this UserManager<TUser> userManager, TUser user, string token) where TUser : class
        {
            return userManager.VerifyUserTokenAsync(user, FireGiantTokenProviderConstants.PasswordlessLoginTokenProvider, FireGiantTokenProviderConstants.PasswordlessLoginPurpose, token);
        }

        public static Task<bool> VerifyPasswordlessShortAccessToken<TUser>(this UserManager<TUser> userManager, TUser user, string token) where TUser : class
        {
            return userManager.VerifyUserTokenAsync(user, FireGiantTokenProviderConstants.PasswordlessLoginTotpTokenProvider, FireGiantTokenProviderConstants.PasswordlessLoginPurpose, token);
        }
    }
}
