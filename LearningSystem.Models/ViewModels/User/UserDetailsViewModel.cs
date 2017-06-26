using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.User
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }

        [DisplayName("Username")]
        public string CUsername { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Name { get; set; }

        [DisplayName("Birth date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
