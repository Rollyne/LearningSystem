using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LearningSystem.Services
{
    public class CoursesService<TUnitOfWork> : Service<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public CoursesService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public CoursesService()
            :base(new TUnitOfWork())
        {
        }

        public Tuple<List<CourseIndexViewModel>, int> GetAllCoursesFiltered(CourseFilterViewModel courseFilter)
        {
            Expression<Func<Course, bool>> where = course => true;
            if (!string.IsNullOrEmpty(courseFilter.Search))
            {
                if (courseFilter.SearchInDescription && courseFilter.SearchInName)
                {
                    where = c => c.Name.Contains(courseFilter.Search) || c.Description.Contains(courseFilter.Search);
                }
                else if (courseFilter.SearchInDescription)
                {
                    where = c => c.Description.Contains(courseFilter.Search);
                }
                else if (courseFilter.SearchInName)
                {
                    where = c => c.Name.Contains(courseFilter.Search);
                }
            }
            
            var corsesAndPages = unitOfWork.GetRepository<Course>()
                .GetAllPaged(
                page: courseFilter.Page,
                itemsPerPage: courseFilter.ItemsPerPage == 0 ? ApplicationConstants.DefaultItemsPerPage : courseFilter.ItemsPerPage,
                where: where,
                select: c => new CourseIndexViewModel()
                {
                    Name = c.Name,
                    Description = c.Description,
                    TrainerName = c.Trainer.Name,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate
                });

            return corsesAndPages;
        }

        public CourseDetailsViewModel GetById(int id)
        {
            var course = unitOfWork.GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == id,
                    select: c => new CourseDetailsViewModel()
                    {
                        Name = c.Name,
                        Description = c.Description,
                        EndDate = c.EndDate,
                        StartDate = c.StartDate,
                        StudentsCount = c.StudentsRelationships.Count,
                        TrainerName = c.Trainer.Name
                    });

            return course;
        }

        //TODO: Make it return appropriate message | Consider already signed up
        public bool SignUpToCourse(int courseId, string studentId)
        {
            var courseStartDate = unitOfWork
                .GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == courseId,
                    select: c => c.StartDate);

            if (courseStartDate.CompareTo(DateTime.Now) == -1)
            {
                return false;
            }

            unitOfWork.GetRepository<StudentsCourses>().Add(new StudentsCourses()
            {
                CourseId = courseId,
                StudentId = studentId
            });

            unitOfWork.Save();

            return true;
        }
        public bool SignOutOfCourse(int courseId, string studentId)
        {
            var courseStartDate = unitOfWork
                .GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == courseId,
                    select: c => c.StartDate);

            if (courseStartDate.CompareTo(DateTime.Now) == -1)
            {
                return false;
            }

            var relationshipRepo = unitOfWork.GetRepository<StudentsCourses>();
            
            var relationship = relationshipRepo.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
            if (relationship == default(StudentsCourses))
                return false;

            relationshipRepo.Delete(relationship);

            unitOfWork.Save();

            return true;
        }
    }
}
