// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace FireGiant.Identity.AzureTableStorage
{
    internal class AzureUserReference : TableEntity
    {
        public AzureUserReference()
        {
        }

        public AzureUserReference(AzureUserReferenceKey keys, string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("User id must be specified to create a reference.", nameof(id));
            }

            this.PartitionKey = keys.Partition;
            this.RowKey = keys.Row;
            this.UserId = id;
        }

        public string UserId { get; set; }
    }
}