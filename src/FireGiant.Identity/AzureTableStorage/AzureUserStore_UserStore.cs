// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.WindowsAzure.Storage.Table;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IUserStore<TUser>
    {
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.SetEntityKeys();

            var operations = new List<TableOperation> { TableOperation.Insert(user) };
            operations.AddRange(user.ReferencesToAdd.Select(TableOperation.Insert));
            operations.AddRange(user.ReferencesToRemove.Select(TableOperation.Delete));

            await this.ExecuteOperationsAsync(operations);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            user.SetEntityKeys();

            user.RemoveReference(CreateUserNameReference(user));
            user.RemoveReference(CreateEmailReference(user));

            var operations = new List<TableOperation> { TableOperation.Delete(user) };
            operations.AddRange(user.ReferencesToRemove.Select(TableOperation.Delete));

            await this.ExecuteOperationsAsync(operations);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.SetEntityKeys();
            user.ETag = "*";

            var operations = new List<TableOperation> { TableOperation.Replace(user) };
            operations.AddRange(user.ReferencesToAdd.Select(TableOperation.Insert));
            operations.AddRange(user.ReferencesToRemove.Select(TableOperation.Delete));

            await this.ExecuteOperationsAsync(operations);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var key = AzureUserKey.ForUserId(userId);

            var op = TableOperation.Retrieve<TUser>(key.Partition, key.Row);

            var result = await this.ExecuteAsync(op);

            return (TUser)result.Result;
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var referenceKey = AzureUserReferenceKey.ForUsername(normalizedUserName);
            return await this.GetUserByReference(referenceKey);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            if (user.NormalizedUserName != normalizedName)
            {
                if (!String.IsNullOrEmpty(user.NormalizedUserName))
                {
                    user.RemoveReference(CreateUserNameReference(user));
                }

                user.NormalizedUserName = normalizedName;
                user.AddReference(CreateUserNameReference(user));
            }

            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        private static AzureUserReference CreateUserNameReference(AzureUser user)
        {
            if (String.IsNullOrEmpty(user.NormalizedUserName))
            {
                return null;
            }

            var referenceKey = AzureUserReferenceKey.ForUsername(user.NormalizedUserName);
            return new AzureUserReference(referenceKey, user.Id) { ETag = "*" };
        }
    }
}