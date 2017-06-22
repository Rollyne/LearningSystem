using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSystem.Models.EntityModels
{
    public class Course
    {
        private ICollection<StudentsCourses> students;

        public Course()
        {
            students = new HashSet<StudentsCourses>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("Trainer")]
        public string TrainerId { get; set; }
        public ApplicationUser Trainer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<StudentsCourses> StudentsRelationships
        {
            get { return students; }
            set { students = value; }
        }
    }
}
