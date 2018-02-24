﻿using System.ComponentModel.DataAnnotations;

namespace CoreCrm.Models.ViewModels.Security
{
    public class LoginViewModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}