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
using LearningSystem.Services.Tools.Generic;
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

        public IExecutionResult<Tuple<List<CourseIndexViewModel>, int>> GetAllCoursesFiltered(CourseFilterViewModel courseFilter)
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
            
            var coursesAndPages = unitOfWork.GetRepository<Course>()
                .GetAllPaged(
                page: courseFilter.Page,
                itemsPerPage: courseFilter.ItemsPerPage == 0 ? ApplicationConstants.DefaultItemsPerPage : courseFilter.ItemsPerPage,
                where: where,
                select: c => new CourseIndexViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    TrainerName = c.Trainer.Name,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate
                }, orderBy: c => c.Id);
            var result = new ExecutionResult<Tuple<List<CourseIndexViewModel>, int>>()
            {
                Result = coursesAndPages,
                Message = "",
                Succeded = true
            };
            return result;
        }

        public IExecutionResult<CourseDetailsViewModel> GetById(int courseId, string userId)
        {
            var result = new ExecutionResult<CourseDetailsViewModel>();

            var course = unitOfWork.GetRepository<Course>()
                .FirstOrDefault(
                    where: c => c.Id == courseId,
                    select: c => new CourseDetailsViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        EndDate = c.EndDate,
                        StartDate = c.StartDate,
                        StudentsCount = c.StudentsRelationships.Count,
                        TrainerName = c.Trainer.Name,
                        IsCurrentUserSignedUp = c.StudentsRelationships.Any(r => r.StudentId == userId),
                        IsCurrentUserTrainer = c.Trainer.Id == userId
                    });
            if (course == null)
            {
                result.Succeded = false;
                result.Message = CourseMessages.NotFound();
                result.Result = null;

                return result;
            }
            result.Result = course;
            result.Message = "";
            result.Succeded = true;

            return result;
        }

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
                    StudentId = studentId,
                    CourseId = courseId,
                    Course = unitOfWork.GetRepository<Course>().FirstOrDefault(c => c.Id == courseId),
                    Student = unitOfWork.GetRepository<Student>().FirstOrDefault(s => s.Id == studentId)
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

        public IExecutionResult<Tuple<List<CourseIndexViewModel>, int>>  GetStudentCourses(string id, int page)
        {
            var execution = new ExecutionResult<Tuple<List<CourseIndexViewModel>, int>>();

            var repo = unitOfWork.GetRepository<Course>();
            execution.Result = repo.GetAllPaged(
                where: c => c.StudentsRelationships.Any(r => r.StudentId == id),
                orderBy: c => c.StartDate,
                select: c => new CourseIndexViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    TrainerName = c.Trainer.Name
                },
                page: page,
                itemsPerPage: ApplicationConstants.DefaultItemsPerPage
            );

            execution.Succeded = true;

            return execution;
        }

        public IExecutionResult GradeStudent(GradeStudentViewModel model, string userId)
        {
            var repo = unitOfWork.GetRepository<StudentsCourses>();
            if (
                !repo.Any(
                    r => r.Course.Trainer.Id == userId && r.StudentId == model.StudentId && r.CourseId == model.CourseId))
            {
                return new ExecutionResult()
                {
                    Succeded = false,
                    Message = CourseMessages.CannotGrade()
                };
            }

            var relationship = repo.FirstOrDefault(where: r => r.StudentId == model.StudentId && r.CourseId == model.CourseId);
            relationship.Grade = model.Grade;

            repo.Update(relationship);
            unitOfWork.Save();

            return new ExecutionResult()
            {
                Succeded = true,
                Message = CourseMessages.SuccessfullyGraded()
            };
        }
    }
}
