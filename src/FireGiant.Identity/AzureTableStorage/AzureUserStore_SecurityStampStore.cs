// Copyright (c) FireGiant.  All Rights Reserved.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserSecurityStampStore<TUser>
    {
        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }
    }
}