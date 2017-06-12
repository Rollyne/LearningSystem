using System;

namespace LearningSystem.Models.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string TrainerName { get; set; }

        public int StudentsCount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
