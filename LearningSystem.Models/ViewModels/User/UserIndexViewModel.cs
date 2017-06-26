using System.ComponentModel;

namespace LearningSystem.Models.ViewModels.User
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }

        [DisplayName("Username")]
        public string CUsername { get; set; }

        public string Name { get; set; }
    }
}
