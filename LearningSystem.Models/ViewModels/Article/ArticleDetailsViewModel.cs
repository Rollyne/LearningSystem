using System;

namespace LearningSystem.Models.ViewModels.Article
{
    public class ArticleDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorName { get; set; }
    }
}
