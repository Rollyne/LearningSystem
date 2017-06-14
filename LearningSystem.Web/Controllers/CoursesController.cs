using System.Web.Mvc;
using LearningSystem.Data;
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
            var modelPagesPair = Service.GetAllCoursesFiltered(filter);
            var model = modelPagesPair.Item1;
            ViewBag.Pages = modelPagesPair.Item2;
            return View(model);
        }
        
        public ActionResult Details(int id)
        {
            var model = Service.GetById(id, HttpContext.User.Identity.GetUserId());

            if (model == null)
                return HttpNotFound();

            return View(model);
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
    }
}
