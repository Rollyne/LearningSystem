namespace LearningSystem.Models.ViewModels.Filtering
{
    public class UserFilterViewModel : IPagerFilter
    {
        public int ItemsPerPage { get; set; }
        public int Page { get; set; }
    }
}
