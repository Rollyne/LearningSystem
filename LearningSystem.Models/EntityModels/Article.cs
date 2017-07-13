using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSystem.Models.EntityModels
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [Display(Name = "Published on")]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        [Display(Name = "Author")]
        public ApplicationUser Author { get; set; }
    }
}
