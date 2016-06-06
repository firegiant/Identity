// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace FireGiant.Identity.AzureTableStorage
{
    public class AzureUser : TableEntity
    {
        public string Id { get; set; }

        /// <summary>
        /// Username chosen by the user.
        /// </summary>
        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        /// <summary>
        /// Email for the user.
        /// </summary>
        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        /// <summary>
        /// True if the email is confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials change (password changed, login removed).
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that should change whenever a user is persisted to the store.
        /// </summary>
        public virtual string ConcurrencyStamp { get; } = Guid.NewGuid().ToString("N").ToLowerInvariant();

        /// <summary>
        /// Phone number for the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user.
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Is lockout enabled for this user.
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout.
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Encoded set of claims for this user.
        /// </summary>
        public string Claims { get; set; }

        /// <summary>
        /// Encoded set of external logins for this user.
        /// </summary>
        public string Logins { get; set; }

        /// <summary>
        /// Encoded set of tokens for this user.
        /// </summary>
        public string Tokens { get; set; }

        internal List<AzureUserReference> ReferencesToAdd { get; } = new List<AzureUserReference>();

        internal List<AzureUserReference> ReferencesToRemove { get; } = new List<AzureUserReference>();

        protected static TUser NewUser<TUser>(string username, string email) where TUser : AzureUser, new()
        {
            return new TUser
            {
                Id = Guid.NewGuid().ToString("N").ToLowerInvariant(),
                UserName = username,
                Email = email
            };
        }

        internal void AddReference(AzureUserReference reference)
        {
            if (reference != null)
            {
                this.ReferencesToAdd.Add(reference);
            }
        }

        internal void RemoveReference(AzureUserReference reference)
        {
            if (reference != null)
            {
                this.ReferencesToRemove.Add(reference);
            }
        }

        internal void RemoveReferences(IEnumerable<AzureUserReference> references)
        {
            if (references != null)
            {
                this.ReferencesToRemove.AddRange(references);
            }
        }

        internal virtual AzureUser SetEntityKeys()
        {
            var key = AzureUserKey.ForUserId(this.Id);

            this.PartitionKey = key.Partition;
            this.RowKey = key.Row;

            return this;
        }
    }
}