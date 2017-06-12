using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSystem.Models.EntityModels
{
    public class Student
    {
        private ICollection<Course> courses;

        public Student()
        {
            courses = new HashSet<Course>();
        }

        [Key, ForeignKey("User")]
        public string Id { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<Course> Courses
        {
            get { return this.courses; }
            set { this.courses = value; }
        }
    }
}
