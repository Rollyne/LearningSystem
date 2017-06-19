using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.Course
{
    public class GradeStudentViewModel
    {
        public int CourseId { get; set; }

        [DisplayName("Student Id")]
        public string StudentId { get; set; }

        [Range(2, 6)]
        public int Grade { get; set; }
    }
}
