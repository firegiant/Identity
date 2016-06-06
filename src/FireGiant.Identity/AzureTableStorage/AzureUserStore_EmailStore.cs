// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserEmailStore<TUser>
    {
        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var referenceKey = AzureUserReferenceKey.ForEmail(normalizedEmail);
            return await this.GetUserByReference(referenceKey);
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            if (user.NormalizedEmail != normalizedEmail)
            {
                if (!String.IsNullOrEmpty(user.NormalizedEmail))
                {
                    user.RemoveReference(CreateEmailReference(user));
                }

                user.NormalizedEmail = normalizedEmail;
                user.AddReference(CreateEmailReference(user));
            }

            return Task.FromResult(0);
        }

        private static AzureUserReference CreateEmailReference(TUser user)
        {
            if (String.IsNullOrEmpty(user.NormalizedEmail))
            {
                return null;
            }

            var referenceKey = AzureUserReferenceKey.ForEmail(user.NormalizedEmail);
            return new AzureUserReference(referenceKey, user.Id) { ETag = "*" };
        }
    }
}