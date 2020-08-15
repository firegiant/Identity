// Copyright (c) FireGiant.  All Rights Reserved.

using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FireGiant.Identity.TokenProviders
{
    public class PasswordlessLoginTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public PasswordlessLoginTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<PasswordlessLoginTokenProviderOptions> options)
            : base(dataProtectionProvider, options)
        {
        }

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public async override Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var token = await base.GenerateAsync(purpose, manager, user);
            return token.Replace('+', '-').Replace('/', '_');
        }

        public override Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            token = token.Replace('-', '+').Replace('_', '/');
            return base.ValidateAsync(purpose, token, manager, user);
        }
    }
}
