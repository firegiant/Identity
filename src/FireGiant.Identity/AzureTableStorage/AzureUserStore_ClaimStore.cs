// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserClaimStore<TUser>
    {
        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var existingClaims = NameValue.Parse(user.Claims).ToList();

            existingClaims.RemoveAll(c => c.Name == claim.Type && c.Value == claim.Value);

            // TODO: should the add only happen if claim was removed?
            existingClaims.Add(new NameValue { Name = claim.Type, Value = claim.Value });

            user.Claims = NameValue.Generate(existingClaims);

            return Task.FromResult(0);
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var nvs = claims.Select(c => new NameValue { Name = c.Type, Value = c.Value });

            var existingClaims = NameValue.Parse(user.Claims)
                .ToList();

            existingClaims.AddRange(nvs);

            user.Claims = NameValue.Generate(existingClaims);

            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            var nameValues = NameValue.Parse(user.Claims);

            var claims = (IList<Claim>)nameValues.Select(nv => new Claim(nv.Name, nv.Value)).ToList();

            return Task.FromResult(claims);
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var existingClaims = NameValue.Parse(user.Claims).ToList();

            // TODO: This isn't efficient but it works.
            foreach (var claim in claims)
            {
                existingClaims.RemoveAll(c => c.Name == claim.Type && c.Value == claim.Value);
            }

            user.Claims = NameValue.Generate(existingClaims);

            return Task.FromResult(0);
        }

        public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            // TODO: do we need to implement this? Don't really want to.
            throw new NotImplementedException();
        }
    }
}