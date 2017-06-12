using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSystem.Models.EntityModels
{
    public class Student
    {
        private ICollection<StudentsCourses> courses;

        public Student()
        {
            courses = new HashSet<StudentsCourses>();
        }

        [Key, ForeignKey("User")]
        public string Id { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<StudentsCourses> CourseRelationships
        {
            get { return this.courses; }
            set { this.courses = value; }
        }
    }
}
