using System.ComponentModel.DataAnnotations;

namespace Fiver.Security.AspIdentity.Models.ViewModels.Security
{
    public class LoginViewModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}