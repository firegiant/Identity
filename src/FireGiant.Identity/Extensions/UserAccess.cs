// Copyright (c) FireGiant.  All Rights Reserved.

using System;

namespace FireGiant.Identity.Extensions
{
    public class UserAccess 
    {
        public string IP { get; set; }

        public string UserAgent { get; set; }

        public string Reason { get; set; }

        public DateTimeOffset When { get; set; }
    }
}
