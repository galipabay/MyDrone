using System.ComponentModel.DataAnnotations;

namespace MyDrone.Web.App.Models
{
    public class ResetPasswordViewModel

    {
        public string Token { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi girin.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalı.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifreyi doğrulayın.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
    }
}
