// Copyright (c) FireGiant.  All Rights Reserved.

using System;
using Microsoft.AspNetCore.Identity;

namespace FireGiant.Identity.TokenProviders
{
    public class PasswordlessLoginTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public PasswordlessLoginTokenProviderOptions()
        {
            this.Name = FireGiantTokenProviderConstants.PasswordlessLoginTokenProvider;
            this.TokenLifespan = TimeSpan.FromMinutes(15);
        }
    }
}
