﻿using System;
using LearningSystem.Models.EntityModels;

namespace LearningSystem.Models.ViewModels.Course
{
    public class CourseIndexViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string TrainerName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
