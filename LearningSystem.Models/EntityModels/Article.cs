using System;

namespace LearningSystem.Models.EntityModels
{
    public class Article
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
