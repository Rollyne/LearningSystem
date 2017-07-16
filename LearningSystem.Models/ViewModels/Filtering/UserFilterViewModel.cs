using System.ComponentModel;

namespace LearningSystem.Models.ViewModels.Filtering
{
    public class UserFilterViewModel : IPagerFilter
    {
        public int? ItemsPerPage { get; set; }
        public int Page { get; set; } = 1;

        public string Search { get; set; }

        [DisplayName("Username")]
        public bool? SearchInUsername { get; set; } = false;

        [DisplayName("Email")]
        public bool? SearchInEmail { get; set; } = false;

        [DisplayName("Name")]
        public bool? SearchInName { get; set; } = false;
    }
}
