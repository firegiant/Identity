// Copyright (c) FireGiant.  All Rights Reserved.

using System;

namespace FireGiant.Identity.AzureTableStorage
{
    internal class AzureUserReferenceKey
    {
        private AzureUserReferenceKey(string referenceKey)
        {
            if (String.IsNullOrEmpty(referenceKey)) throw new ArgumentNullException(nameof(referenceKey));

            this.Partition = "ref|" + referenceKey;
            this.Row = String.Empty;
        }

        private AzureUserReferenceKey(string tenant, string referenceKey)
        {
            if (String.IsNullOrEmpty(tenant)) throw new ArgumentNullException(nameof(tenant));
            if (String.IsNullOrEmpty(referenceKey)) throw new ArgumentNullException(nameof(referenceKey));

            this.Partition = "ref|tenant=" + tenant + "|" + referenceKey;
            this.Row = String.Empty;
        }

        public string Partition { get; }

        public string Row { get; }

        public static AzureUserReferenceKey ForUsername(string username)
        {
            return new AzureUserReferenceKey("user=" + username.ToBase64IfUnsafe());
        }

        public static AzureUserReferenceKey ForUsername(string tenant, string username)
        {
            return new AzureUserReferenceKey(tenant, "user=" + username.ToBase64IfUnsafe());
        }

        public static AzureUserReferenceKey ForEmail(string email)
        {
            return new AzureUserReferenceKey("email=" + email.ToBase64IfUnsafe());
        }

        public static AzureUserReferenceKey ForEmail(string tenant, string email)
        {
            return new AzureUserReferenceKey(tenant, "email=" + email.ToBase64IfUnsafe());
        }

        public static AzureUserReferenceKey ForPhoneNumber(string tenant, string phone)
        {
            return new AzureUserReferenceKey(tenant, "phone=" + phone);
        }

        public static AzureUserReferenceKey ForVerificationKey(string key)
        {
            return new AzureUserReferenceKey("key=" + key);
        }

        public static AzureUserReferenceKey ForLinkedAccount(string tenant, string provider, string id)
        {
            return new AzureUserReferenceKey(tenant, "link=" + provider + "|" + id.ToBase64IfUnsafe());
        }

        public static AzureUserReferenceKey ForCertificate(string tenant, string thumbprint)
        {
            return new AzureUserReferenceKey(tenant, "cert=" + thumbprint);
        }
    }
}