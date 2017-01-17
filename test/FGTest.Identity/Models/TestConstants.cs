using System;

namespace FGTest.Identity.Models
{
    public static class TestConstants
    {
        public static readonly string Id = Guid.NewGuid().ToString("N");

        public const string Email = "test@example.com";

        public const string NormalizedEmail = "TEST@EXAMPLE.COM";

        public const string Password = "P@ssw0rd";

        public const string UserName = "testuser";

        public const string NormalizedUserName = "TESTUSER";
    }
}