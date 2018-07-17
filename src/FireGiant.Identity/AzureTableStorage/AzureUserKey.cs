// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Diagnostics;

namespace FireGiant.Identity.AzureTableStorage
{
    internal class AzureUserKey
    {
        private AzureUserKey(string userId)
        {
            this.Partition = userId;
            this.Row = String.Empty;
        }

        private AzureUserKey(Guid userId)
        {
            this.Partition = CalculatePartitionKeyForUserId(userId);
            this.Row = String.Empty;
        }

        public string Partition { get; }

        public string Row { get; }

        public static AzureUserKey ForUserId(string userId)
        {
#if DEBUG
            if (!Guid.TryParse(userId, out var guid) || userId != CalculatePartitionKeyForUserId(guid))
            {
                Debug.Fail("User id should be a Guid");
            }
#endif

            return new AzureUserKey(userId);
        }

        public static AzureUserKey ForUserId(Guid userId)
        {
            return new AzureUserKey(userId);
        }

        private static string CalculatePartitionKeyForUserId(Guid userId)
        {
            return userId.ToString("N").ToLowerInvariant();
        }
    }
}