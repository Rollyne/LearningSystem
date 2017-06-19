using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSystem.Models.EntityModels
{
    public class StudentsCourses
    {
        [Key, Column(Order=0), ForeignKey("Student")]
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Key, Column(Order=1), ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int? Grade { get; set; }
    }
}
