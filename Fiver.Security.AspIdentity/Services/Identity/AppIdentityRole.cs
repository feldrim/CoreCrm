﻿using Microsoft.AspNetCore.Identity;

namespace Fiver.Security.AspIdentity.Services.Identity
{
    public class AppIdentityRole : IdentityRole
    {
        public string Description { get; set; }
    }
}