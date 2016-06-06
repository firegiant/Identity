// Copyright (c) FireGiant.  All Rights Reserved.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.Extensions
{
    public static class UserManagerExtensions
    {
        public async static Task<TUser> FindByNameOrEmailAsync<TUser>(this UserManager<TUser> userManager, string nameOrEmail) where TUser : class
        {
            return await userManager.FindByNameAsync(nameOrEmail) ?? await userManager.FindByEmailAsync(nameOrEmail);
        }
    }
}