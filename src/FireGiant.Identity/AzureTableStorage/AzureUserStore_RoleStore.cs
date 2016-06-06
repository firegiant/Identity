// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserRoleStore<TUser>
    {
        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var existing = NameValue.Parse(user.Claims).ToList();

            existing.Add(new NameValue { Name = JwtClaimTypes.Role, Value = roleName });

            user.Claims = NameValue.Generate(existing);

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            var roles = NameValue.Parse(user.Claims)
                .Where(c => c.Name == JwtClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return Task.FromResult((IList<string>)roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var inRole = NameValue.Parse(user.Claims)
                .Any(c => c.Name == JwtClaimTypes.Role && c.Value == roleName);

            return Task.FromResult(inRole);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var existing = NameValue.Parse(user.Claims).ToList();

            existing.RemoveAll(r => r.Name == JwtClaimTypes.Role && r.Value == roleName);

            user.Claims = NameValue.Generate(existing);

            return Task.FromResult(0);
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            // TODO: do we need to implement this? Don't really want to.
            throw new NotImplementedException();
        }
    }
}