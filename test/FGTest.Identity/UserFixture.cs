using System;
using System.IO;
using System.Threading.Tasks;
using FGTest.Identity.Models;
using FireGiant.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FGTest.Identity
{
    public class UserFixture : FixtureBase
    {
        [Fact]
        public async Task CanCreateUser()
        {
            var user = new TestUser()
            {
                Id = Guid.NewGuid().ToString("N"),
                Email = "foo@example.com",
                UserName = "foo"
            };

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            var result = await userManager.CreateAsync(user, "P@ssw0rd");
            Assert.True(result.Succeeded);
            Assert.Equal("FOO", user.NormalizedUserName);
            Assert.Equal("FOO@EXAMPLE.COM", user.NormalizedEmail);
        }

        [Fact]
        public async Task CanFindUserById()
        {
            await this.EnsureTestUserCreated();

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            var found = await userManager.FindByIdAsync(TestConstants.Id);
            Assert.NotNull(found);
            Assert.Equal(TestConstants.Id, found.Id);
            Assert.Equal(TestConstants.Email, found.Email);
            Assert.Equal(TestConstants.NormalizedEmail, found.NormalizedEmail);
            Assert.Equal(TestConstants.UserName, found.UserName);
            Assert.Equal(TestConstants.NormalizedUserName, found.NormalizedUserName);
        }

        [Fact]
        public async Task CanFindUserByEmail()
        {
            await this.EnsureTestUserCreated();

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            var found = await userManager.FindByEmailAsync(TestConstants.Email);
            Assert.NotNull(found);
            Assert.Equal(TestConstants.Id, found.Id);
            Assert.Equal(TestConstants.Email, found.Email);
            Assert.Equal(TestConstants.NormalizedEmail, found.NormalizedEmail);
            Assert.Equal(TestConstants.UserName, found.UserName);
            Assert.Equal(TestConstants.NormalizedUserName, found.NormalizedUserName);
        }

        [Fact]
        public async Task CanFindUserByUserName()
        {
            await this.EnsureTestUserCreated();

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            var found = await userManager.FindByNameAsync(TestConstants.UserName);
            Assert.NotNull(found);
            Assert.Equal(TestConstants.Id, found.Id);
            Assert.Equal(TestConstants.Email, found.Email);
            Assert.Equal(TestConstants.NormalizedEmail, found.NormalizedEmail);
            Assert.Equal(TestConstants.UserName, found.UserName);
            Assert.Equal(TestConstants.NormalizedUserName, found.NormalizedUserName);
        }

        [Fact]
        public async Task CanFindUserByUserNameOrEmail()
        {
            await this.EnsureTestUserCreated();

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            var foundByName = await userManager.FindByNameOrEmailAsync(TestConstants.UserName);
            Assert.NotNull(foundByName);
            Assert.Equal(TestConstants.Id, foundByName.Id);
            Assert.Equal(TestConstants.Email, foundByName.Email);
            Assert.Equal(TestConstants.NormalizedEmail, foundByName.NormalizedEmail);
            Assert.Equal(TestConstants.UserName, foundByName.UserName);
            Assert.Equal(TestConstants.NormalizedUserName, foundByName.NormalizedUserName);

            var foundByEmail = await userManager.FindByNameOrEmailAsync(TestConstants.Email);
            Assert.NotNull(foundByEmail);
            Assert.Equal(TestConstants.Id, foundByEmail.Id);
            Assert.Equal(TestConstants.Email, foundByEmail.Email);
            Assert.Equal(TestConstants.NormalizedEmail, foundByEmail.NormalizedEmail);
            Assert.Equal(TestConstants.UserName, foundByEmail.UserName);
            Assert.Equal(TestConstants.NormalizedUserName, foundByEmail.NormalizedUserName);
        }

        [Fact]
        public async Task CanAddAccess()
        {
            var user = await this.EnsureTestUserCreated();

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            await userManager.AddAccess(user, "127.0.0.1", "Tests", "Success");

            var accesses = await userManager.GetAccesses(user);
            Assert.NotEmpty(accesses);
        }

        private async Task<TestUser> EnsureTestUserCreated()
        {
            var user = new TestUser()
            {
                Id = TestConstants.Id,
                Email = TestConstants.Email,
                UserName = TestConstants.UserName
            };

            var userManager = this.Scope.ServiceProvider.GetService<UserManager<TestUser>>();

            var result = await userManager.CreateAsync(user, TestConstants.Password);

            if (!result.Succeeded)
            {
                throw new InvalidDataException("Failed to create test user.");
            }

            return user;
        }
    }
}
