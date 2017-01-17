// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace FireGiant.Identity.AzureTableStorage
{
    internal class AzureUserAccess : TableEntity
    {
        public AzureUserAccess()
        {
        }

        public AzureUserAccess(AzureUser user, string ip, string userAgent, string reason, DateTimeOffset when)
        {
            var rowKey = String.IsNullOrEmpty(user.RowKey) ? String.Empty : user.RowKey + "|";
            rowKey += when.ToString("yyyy-MM-ddTHH-mm-ss");

            this.PartitionKey = user.PartitionKey;
            this.RowKey = "access|" + rowKey;

            this.IP = ip;
            this.UserAgent = userAgent;
            this.Reason = reason;
            this.When = when;
        }

        public string IP { get; set; }

        public string UserAgent { get; set; }

        public string Reason { get; set; }

        public DateTimeOffset When { get; set; }
    }
}
