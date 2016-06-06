// Copyright (c) FireGiant.  All Rights Reserved.

using System;

namespace FireGiant.Identity.AzureTableStorage
{
    /// <summary>
    /// Empty role store to appease Identity system. Roles are actually managed as
    /// claims.
    /// </summary>
    /// <typeparam name="TRole">Unused role type.</typeparam>
    public partial class AzureRoleStore<TRole> : IDisposable where TRole : class
    {
        public void Dispose()
        {
        }
    }
}