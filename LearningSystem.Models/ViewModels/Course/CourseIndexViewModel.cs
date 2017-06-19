using System;

namespace LearningSystem.Models.ViewModels.Course
{
    public class CourseIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TrainerName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
