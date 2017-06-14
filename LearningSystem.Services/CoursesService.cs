using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services.Tools;
using LearningSystem.Services.Tools.Messages;

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
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TrainerName = c.Trainer.Name,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate
                }, orderByKeySelector: c => c.Id);

            return corsesAndPages;
        }

        public CourseDetailsViewModel GetById(int id, string studentId)
        {
            var course = unitOfWork.GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == id,
                    select: c => new CourseDetailsViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        EndDate = c.EndDate,
                        StartDate = c.StartDate,
                        StudentsCount = c.StudentsRelationships.Count,
                        TrainerName = c.Trainer.Name,
                        IsCurrentUserSignedUp = c.StudentsRelationships.Any(r => r.StudentId == studentId)
                    });

            return course;
        }

        //TODO: Make it return appropriate message | Consider already signed up
        public IExecutionResult SignUpToCourse(int courseId, string studentId)
        {
            var result = new ExecutionResult();

            var courseStartDate = unitOfWork
                .GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == courseId,
                    select: c => c.StartDate);
            
            if (courseStartDate.CompareTo(DateTime.Now) == -1)
            {
                result.Succeded = false;
                result.Message = CourseMessages.UnsuccessfulSignUpPassedStartDate();
                return result;
            }
            var relationshipRepo = unitOfWork.GetRepository<StudentsCourses>();
            if (relationshipRepo.Any(r => r.CourseId == courseId && r.StudentId == studentId))
            {
                result.Succeded = false;
                result.Message = CourseMessages.AlreadySignedUp();
                return result;
            }
            
            try
            {
                relationshipRepo.Add(new StudentsCourses()
                {
                    CourseId = courseId,
                    StudentId = studentId
                });
                unitOfWork.Save();
            }
            catch(DbException)
            {
                result.Succeded = false;
                result.Message = CourseMessages.UnsuccessfulSignUpDbError();

                return result;
            }

            result.Succeded = true;
            result.Message = CourseMessages.SuccessfulSignUp();

            return result;
        }
        public IExecutionResult SignOutOfCourse(int courseId, string studentId)
        {
            var result = new ExecutionResult();

            var courseStartDate = unitOfWork
                .GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == courseId,
                    select: c => c.StartDate);

            if (courseStartDate.CompareTo(DateTime.Now) == -1)
            {
                result.Succeded = false;
                result.Message = CourseMessages.UnsuccessfulSignUpPassedStartDate();
                return result;
            }

            var relationshipRepo = unitOfWork.GetRepository<StudentsCourses>();
            if (!relationshipRepo.Any(r => r.CourseId == courseId && r.StudentId == studentId))
            {
                result.Succeded = false;
                result.Message = CourseMessages.NotSignedUp();
                return result;
            }
            
            var relationship = relationshipRepo.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
            try
            {
                relationshipRepo.Delete(relationship);

                unitOfWork.Save();
            }
            catch (DbException)
            {
                result.Succeded = false;
                result.Message = CourseMessages.UnsuccessfulSignOutDbError();

                return result;
            }

            result.Succeded = true;
            result.Message = CourseMessages.SuccessfulSignOut();

            return result;
        }
    }
}
