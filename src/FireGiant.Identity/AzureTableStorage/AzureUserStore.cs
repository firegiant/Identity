// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> : IDisposable where TUser : AzureUser
    {
        private readonly CloudTable _table;
        private bool _disposed;
        private bool _tableCreated;

        public AzureUserStore(AzureUserStoreConfig config)
            : this(CloudStorageAccount.Parse(config.TableStorageConnectionString), config)
        {
        }

        public AzureUserStore(CloudStorageAccount storage, AzureUserStoreConfig config)
        {
            var client = storage.CreateCloudTableClient();
            _table = client.GetTableReference(config.TableName);
        }

        public void Dispose()
        {
            _disposed = true;
        }

        private async Task<TableQuerySegment<T>> QueryAsync<T>(TableQuery<T> query, TableContinuationToken token) where T : ITableEntity, new()
        {
            this.ThrowIfDisposed();

            if (!_tableCreated)
            {
                await _table.CreateIfNotExistsAsync();

                _tableCreated = true;
            }

            return await _table.ExecuteQuerySegmentedAsync(query, token);
        }

        private async Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            this.ThrowIfDisposed();

            if (!_tableCreated)
            {
                await _table.CreateIfNotExistsAsync();

                _tableCreated = true;
            }

            return await _table.ExecuteAsync(operation);
        }

        private async Task ExecuteOperationsAsync(IEnumerable<TableOperation> operations)
        {
            this.ThrowIfDisposed();

            if (!_tableCreated)
            {
                await _table.CreateIfNotExistsAsync();

                _tableCreated = true;
            }

            foreach (var op in operations.Where(op => op != null))
            {
                var result = await _table.ExecuteAsync(op);

                // TODO: error if result.Result is less than desirable.
            }
        }

        private async Task<TUser> GetUserByReference(AzureUserReferenceKey referenceKey)
        {
            this.ThrowIfDisposed();

            var op = TableOperation.Retrieve<AzureUserReference>(referenceKey.Partition, referenceKey.Row);

            var result = await this.ExecuteAsync(op);

            if (result.HttpStatusCode != 200)
            {
                return null;
            }

            var reference = (AzureUserReference)result.Result;

            if (reference.UserId == null)
            {
                // TODO: log error because this should never happen.

                reference.UserId = String.Empty;
            }

            var userKey = AzureUserKey.ForUserId(reference.UserId);

            op = TableOperation.Retrieve<TUser>(userKey.Partition, userKey.Row);

            result = await this.ExecuteAsync(op);

            return (TUser)result.Result;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AzureUserStore<TUser>));
            }
        }
    }
}