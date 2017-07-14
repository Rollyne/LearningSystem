using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.Article
{
    public class ArticleModifyViewModel
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public DateTime? PublishDate { get; set; }
    }
}
