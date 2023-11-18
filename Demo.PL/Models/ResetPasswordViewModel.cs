using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "The Password Length Must be 8")]
        public string Password { get; set; }
        
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password Mismatch")]
        public string ConfirmPassword { get; set; }
        
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
