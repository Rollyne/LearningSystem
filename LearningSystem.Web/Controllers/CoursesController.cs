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
            var model = Service.GetAllCoursesFiltered(filter);
            return View(model);
        }
        
        public ActionResult Details(int id)
        {
            var model = Service.GetById(id);

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [Authorize(Roles = "Student")]
        public ActionResult SignUp(int id)
        {
            if (!Service.SignUpToCourse(id, HttpContext.User.Identity.GetUserId()))
            {
                ModelState.AddModelError("Unsuccessful signing up", "Couldn't sign up to the course");
            }
            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public ActionResult SignOut(int id)
        {
            if (!Service.SignOutOfCourse(id, HttpContext.User.Identity.GetUserId()))
            {
                ModelState.AddModelError("Unsuccessful signing out", "Couldn't sign out of the course");
            }

            return RedirectToAction("Index");
        }
    }
}
