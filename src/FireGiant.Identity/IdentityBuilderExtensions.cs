// Copyright (c) FireGiant.  All Rights Reserved.

using FireGiant.Identity.AzureTableStorage;
using FireGiant.Identity.TokenProviders;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FireGiant.Identity
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddAzureTableStore(this IdentityBuilder builder, string connectionString)
        {
            return AddAzureTableStore(builder, new AzureUserStoreConfig(connectionString));
        }

        public static IdentityBuilder AddAzureTableStore(this IdentityBuilder builder, AzureUserStoreConfig config)
        {
            var userManagerType = typeof(AzureUserManager<>).MakeGenericType(builder.UserType);
            var userStoreType = typeof(AzureUserStore<>).MakeGenericType(builder.UserType);
            var roleStoreType = typeof(AzureRoleStore<>).MakeGenericType(builder.RoleType);

            builder.Services.TryAddSingleton(config);

            builder.Services
                .AddScoped(typeof(UserManager<>).MakeGenericType(builder.UserType), userManagerType);

            builder.Services
                .AddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserAuthenticationTokenStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserClaimStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserEmailStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserLockoutStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserLoginStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserPhoneNumberStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserRoleStore<>).MakeGenericType(builder.UserType), userStoreType)
                .AddScoped(typeof(IUserSecurityStampStore<>).MakeGenericType(builder.UserType), userStoreType);

            builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);

            return builder;
        }

        public static IdentityBuilder AddPasswordlessLoginTokenProvider(this IdentityBuilder builder)
        {
            var provider = typeof(PasswordlessLoginTokenProvider<>).MakeGenericType(builder.UserType);
            return builder.AddTokenProvider(FireGiantTokenProviderConstants.PasswordlessLoginTokenProvider, provider);
        }

        public static IdentityBuilder AddPasswordlessLoginTotpTokenProvider(this IdentityBuilder builder)
        {
            var totpProvider = typeof(PasswordlessLoginTotpTokenProvider<>).MakeGenericType(builder.UserType);
            return builder.AddTokenProvider(FireGiantTokenProviderConstants.PasswordlessLoginTotpTokenProvider, totpProvider);
        }
    }
}
