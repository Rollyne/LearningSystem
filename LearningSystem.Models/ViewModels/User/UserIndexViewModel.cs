using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.User
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Username")]
        public string CUsername { get; set; }
    }
}
