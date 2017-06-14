using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name="Trainer")]
        public string TrainerName { get; set; }

        [Display(Name="Students count")]
        public int StudentsCount { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="End date")]
        public DateTime EndDate { get; set; }

        public bool IsCurrentUserSignedUp { get; set; }
    }
}
