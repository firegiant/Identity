// Copyright (c) FireGiant.  All Rights Reserved.

using System;

namespace FireGiant.Identity.AzureTableStorage
{
    public class AzureUserStoreConfig
    {
        public AzureUserStoreConfig()
            : this(null)
        {
        }

        public AzureUserStoreConfig(string connectionString, string tableName = null)
        {
            this.TableStorageConnectionString = connectionString;

            this.TableName = String.IsNullOrEmpty(tableName) ? "user" : tableName;
        }

        public string TableStorageConnectionString { get; set; }

        public string TableName { get; set; }
    }
}