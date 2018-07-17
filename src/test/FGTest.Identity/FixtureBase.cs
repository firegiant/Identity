// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using FGTest.Identity.Models;
using FireGiant.Identity;
using FireGiant.Identity.AzureTableStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FGTest.Identity
{
    public abstract class FixtureBase : IDisposable
    {
        private readonly CloudTable _table;

        protected FixtureBase(string connection = "UseDevelopmentStorage=true;")
        {
            this.UserTableName = "user" + Guid.NewGuid().ToString("N");

            var services = new ServiceCollection();

            services.AddScoped<UserManager<TestUser>, AzureUserManager<TestUser>>();

            services.AddLogging()
                .AddIdentity<TestUser, TestRole>()
                .AddAzureTableStore(new AzureUserStoreConfig(connection, this.UserTableName));

            this.Scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

            _table = CloudStorageAccount
                .Parse(connection)
                .CreateCloudTableClient()
                .GetTableReference(this.UserTableName);
        }

        protected IServiceScope Scope { get; }

        protected string UserTableName { get; }

        public void Dispose()
        {
            this.Scope.Dispose();

            _table.DeleteIfExistsAsync();
        }
    }
}
