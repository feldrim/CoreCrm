using Microsoft.AspNetCore.Identity;

namespace Fiver.Security.AspIdentity.Models.Core
{
    public class AppIdentityRole : IdentityRole
    {
        public string Description { get; set; }
    }
}