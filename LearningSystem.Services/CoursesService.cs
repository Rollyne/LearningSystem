using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services.Generic;
using LearningSystem.Services.Tools;
using LearningSystem.Services.Tools.Generic;
using LearningSystem.Services.Tools.Messages;

namespace LearningSystem.Services
{
    public class CoursesService<TUnitOfWork> :
        CrudService<TUnitOfWork, CourseModifyViewModel, CourseIndexViewModel, CourseDetailsViewModel, CourseFilterViewModel, Course> 
        where TUnitOfWork : IUnitOfWork, new()
    {
        public CoursesService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public CoursesService()
            :base(new TUnitOfWork())
        {
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
            var checks = repo.FirstOrDefault(
                where: r => r.StudentId == model.StudentId && r.CourseId == model.CourseId,
                select: r => new
            {
                IsTrainer = r.Course.Trainer.Id == userId,
                CourseEndDate = r.Course.EndDate
            });
            if (checks == null)
            {
                return new ExecutionResult()
                {
                    Succeded = false,
                    Message = CourseMessages.CannotGradeNotSignedUp()
                };
            }

            if (!checks.IsTrainer)
            {
                return new ExecutionResult()
                {
                    Succeded = false,
                    Message = CourseMessages.NotTrainerInCourse()
                };
            }

            if (checks.CourseEndDate.CompareTo(DateTime.Now) > 0)
            {
                return new ExecutionResult()
                {
                    Succeded = false,
                    Message = CourseMessages.CannotGradeEndDateHasntPassed()
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

        public IExecutionResult<CourseModifyViewModel> GetForModification(int id)
        {
            var repo = unitOfWork.GetRepository<Course>();

            return base.getForModification(where: c => c.Id == id);
        }

        public IExecutionResult Delete(int id)
        {
            var repo = unitOfWork.GetRepository<Course>();
            return base.Delete(repo.FirstOrDefault(where: c => c.Id == id));
        }

        protected override Course ParseModifyViewModelToEntity(CourseModifyViewModel model)
        {
            return new Course()
            {
                Name = model.Name,
                Description = model.Description,
                Id = model.Id.Value,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                TrainerId = model.TrainerId
            };
        }

        protected override Expression<Func<Course, CourseIndexViewModel>> SelectIndexViewModelQuery =>
            c => new CourseIndexViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                TrainerName = c.Trainer.Name,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            };

        protected override Expression<Func<Course, CourseDetailsViewModel>> SelectDetailsViewModelQuery =>
            c => new CourseDetailsViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                EndDate = c.EndDate,
                StartDate = c.StartDate,
                TrainerName = c.Trainer.Name,
                StudentsCount = c.StudentsRelationships.Count
            };

        protected override Expression<Func<Course, CourseModifyViewModel>> SelectModifyViewModelQuery =>
            c => new CourseModifyViewModel()
            {
                Name = c.Name,
                Description = c.Description,
                EndDate = c.EndDate,
                Id = c.Id,
                StartDate = c.StartDate,
                TrainerId = c.TrainerId
            };

        public override IExecutionResult<Tuple<List<CourseIndexViewModel>, int>> GetAllFiltered(CourseFilterViewModel courseFilter)
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
            return base.getAllFiltered(where: where, filter: courseFilter, order: c => c.Id);
        }

        public IExecutionResult<CourseDetailsViewModel> GetDetails(int courseId, string userId)
        {
            Expression<Func<Course, CourseDetailsViewModel>> query =
                c => new CourseDetailsViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate,
                    TrainerName = c.Trainer.Name,
                    StudentsCount = c.StudentsRelationships.Count,
                    IsCurrentUserSignedUp = c.StudentsRelationships.Any(r => r.StudentId == userId),
                    IsCurrentUserTrainer = c.TrainerId == userId
                };

            return base.getDetails(where: c => c.Id == courseId, select: query);
        }
    }
}
