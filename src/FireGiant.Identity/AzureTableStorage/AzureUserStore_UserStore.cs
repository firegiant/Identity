// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
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

            var operations = new List<TableOperation>();
            operations.Add(TableOperation.Insert(user));
            operations.AddRange(user.ReferencesToAdd.Select(r => TableOperation.Insert(r)));
            operations.AddRange(user.ReferencesToRemove.Select(r => TableOperation.Delete(r)));

            await this.ExecuteOperationsAsync(operations);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            user.SetEntityKeys();

            user.RemoveReference(this.CreateUserNameReference(user));
            user.RemoveReference(CreateEmailReference(user));

            var operations = new List<TableOperation>();
            operations.Add(TableOperation.Delete(user));
            operations.AddRange(user.ReferencesToRemove.Select(r => TableOperation.Delete(r)));

            await this.ExecuteOperationsAsync(operations);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.SetEntityKeys();
            user.ETag = "*";

            var operations = new List<TableOperation>();
            operations.Add(TableOperation.Replace(user));
            operations.AddRange(user.ReferencesToAdd.Select(r => TableOperation.Insert(r)));
            operations.AddRange(user.ReferencesToRemove.Select(r => TableOperation.Delete(r)));

            //var operations = new List<TableOperation>()
            //{
            //    TableOperation.Replace(user)
            //};

            //if (user.PreviousNormalizedUserName != user.NormalizedUserName)
            //{
            //    var oldRef = this.CreateUsernameReference(user, true);
            //    var newRef = this.CreateUsernameReference(user);

            //    if (newRef != null) { operations.Add(TableOperation.Insert(newRef)); }
            //    if (oldRef != null) { operations.Add(TableOperation.Delete(oldRef)); }
            //}

            //if (user.PreviousNormalizedEmail != user.NormalizedEmail)
            //{
            //    var oldRef = this.CreateEmailReference(user, true);
            //    var newRef = this.CreateEmailReference(user);

            //    if (newRef != null) { operations.Add(TableOperation.Insert(newRef)); }
            //    if (oldRef != null) { operations.Add(TableOperation.Delete(oldRef)); }
            //}

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
                    user.RemoveReference(this.CreateUserNameReference(user));
                }

                user.NormalizedUserName = normalizedName;
                user.AddReference(this.CreateUserNameReference(user));
            }

            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        private AzureUserReference CreateUserNameReference(AzureUser user)
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