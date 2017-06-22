using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LearningSystem.Models.Attributes;

namespace LearningSystem.Models.ViewModels.Course
{
    public class CourseModifyViewModel
    {
        public int? Id { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [DisplayName("Trainer Id")]
        [Required]
        public string TrainerId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [FutureDateOnly]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [FutureDateOnly]
        public DateTime EndDate { get; set; }
    }
}
