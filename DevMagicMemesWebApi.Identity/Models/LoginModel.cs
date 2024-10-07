using System;
using System.ComponentModel.DataAnnotations;

namespace DevMagicMemesWebApi.Identity
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
