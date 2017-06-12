using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
