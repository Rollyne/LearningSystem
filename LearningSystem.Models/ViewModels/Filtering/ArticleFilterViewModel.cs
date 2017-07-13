namespace LearningSystem.Models.ViewModels.Filtering
{
    public class ArticleFilterViewModel : IPagerFilter
    {
        public int ItemsPerPage { get; set; }

        public int Page { get; set; } = 1;
    }
}
