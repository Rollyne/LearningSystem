using System;
using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;
using Microsoft.AspNet.Identity;

namespace LearningSystem.Web.Controllers
{
    public class CoursesController : ServiceController<CoursesService<UnitOfWork>>
    {
        public ActionResult Index(CourseFilterViewModel filter)
        {
            var execution = Service.GetAllCoursesFiltered(filter);

            if (!execution.Succeded)
                return HttpNotFound();

            var model = execution.Result.Item1;
            var itemsPerPage = filter.ItemsPerPage == 0 ? ApplicationConstants.DefaultItemsPerPage : filter.ItemsPerPage;
            ViewBag.Pages = Math.Ceiling((double)execution.Result.Item2 / itemsPerPage);
            ViewBag.CurrentPage = filter.Page == 0 ? 1 : filter.Page;

            return View(model);
        }
        
        public ActionResult Details(int id)
        {
            var execution = Service.GetById(id, HttpContext.User.Identity.GetUserId());

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
                ModelState.AddModelError("Sign up", result.Message);
            }
            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public ActionResult SignOut(int id)
        {
            var result = Service.SignOutOfCourse(id, HttpContext.User.Identity.GetUserId());
            if (!result.Succeded)
            {
                ModelState.AddModelError("Sign out", result.Message);
            }

            return RedirectToAction("Index");
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
            var result = Service.GradeStudent(model, HttpContext.User.Identity.GetUserId());

            return Json(!result.Succeded ? $"'Success':'false','Error':'{result.Message}'" : "'Success':'true'");
        }
    }
}
