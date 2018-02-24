using Microsoft.AspNetCore.Identity;

namespace CoreCrm.Models.Core
{
    public class AppIdentityRole : IdentityRole
    {
        public string Description { get; set; }
    }
}