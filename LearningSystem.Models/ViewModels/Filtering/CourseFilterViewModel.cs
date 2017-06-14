﻿namespace LearningSystem.Models.ViewModels.Filtering
{
    public class CourseFilterViewModel
    {
        public string Search { get; set; }

        public int Page { get; set; } = 1;

        public int ItemsPerPage { get; set; }

        public bool SearchInName { get; set; }

        public bool SearchInDescription { get; set; }
    }
}