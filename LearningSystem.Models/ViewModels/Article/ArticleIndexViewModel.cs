using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.Article
{
    public class ArticleIndexViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name="Published on")]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [Display(Name ="Author")]
        public string AuthorName { get; set; }
    }
}
