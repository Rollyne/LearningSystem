namespace LearningSystem.Models.ViewModels.Filtering
{
    public interface IPagerFilter
    {
        int? ItemsPerPage { get; set; }
        int Page { get; set; }
    }
}