using System;
using System.ComponentModel.DataAnnotations;

namespace DevMagicMemesWebApi.Identity
{
    public class ChangePasswordModel
    {
        [Required]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string OldPassword { get; set; } = String.Empty;

        [Required]
        public string NewPassword { get; set; } = String.Empty;
    }
}
