using System;
using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Models.ViewModels.StudentsCourses;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;
using Microsoft.AspNet.Identity;

namespace LearningSystem.Web.Controllers
{
    public class CoursesController : ServiceController<CoursesService<UnitOfWork>>
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult _CoursesListPartial(CourseFilterViewModel filter)
        {
            var execution = Service.GetAllFiltered(filter);

            if (!execution.Succeded)
                return HttpNotFound();

            var model = execution.Result.Item1;
            var itemsPerPage = filter.ItemsPerPage ?? ApplicationConstants.DefaultItemsPerPage;
            TempData["Pages"] = Math.Ceiling((double)execution.Result.Item2 / itemsPerPage);
            TempData["CurrentPage"] = filter.Page == 0 ? 1 : filter.Page;

            return PartialView("_CoursesListPartial", model);
        }

        [HttpGet]
        public ActionResult _SearchPartial(CourseFilterViewModel model)
        {
            return PartialView("_SearchPartial", model ?? new CourseFilterViewModel());
        }

        [HttpGet]
        public ActionResult _PaginationPartial(CourseFilterViewModel model)
        {
            return PartialView("_PaginationPartial", model ?? new CourseFilterViewModel());
        }

        public ActionResult Details(int id)
        {
            var execution = Service.GetDetails(id, HttpContext.User.Identity.GetUserId());

            if (!execution.Succeded)
                return HttpNotFound();

            return View(execution.Result);
        }
        [Authorize(Roles = "Student")]
        public ActionResult SignUp(int id)
        {
            var result = Service.SignUpToCourse(id, HttpContext.User.Identity.GetUserId());
            if (!result.Succeded)
            {
                AddError(result.Message);
            }
            else
            {
                AddMessage(result.Message);
            }
            
            return RedirectToAction("Details", new {id});
        }

        [Authorize(Roles = "Student")]
        public ActionResult SignOut(int id)
        {
            var result = Service.SignOutOfCourse(id, HttpContext.User.Identity.GetUserId());
            if (!result.Succeded)
            {
                AddError(result.Message);
            }
            else
            {
                AddMessage(result.Message);
            }
            return RedirectToAction("Details", new {id});
        }

        [ChildActionOnly]
        public PartialViewResult StudentCourses(int page = 1, string id = null)
        {
            var execution = id == null ? Service.GetStudentCourses(id: HttpContext.User.Identity.GetUserId(), page: page) : Service.GetStudentCourses(id: id, page: page);
            
            var model = execution.Result.Item1;
            ViewBag.Pages = execution.Result.Item2;

            return PartialView("StudentCourses", model);
        }

        [HttpGet]
        [Authorize(Roles = "Trainer")]
        public PartialViewResult GradeStudent(int courseId)
        {
            var model = new GradeStudentViewModel()
            {
                CourseId = courseId
            };

            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public JsonResult GradeStudent(GradeStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json("'Success':'false', 'Error' : 'Invalid grade information! Grades should be between 2 and 6.'");
            }
            var result = Service.GradeStudent(model, HttpContext.User.Identity.GetUserId());
            
            return Json(!result.Succeded ? $"'Success':'false','Error':'{result.Message}'" : "'Success':'true'");
        }

    }
}
