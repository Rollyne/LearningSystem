using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;

namespace LearningSystem.Web.Controllers
{
    public class HomeController : ServiceController<HomeService<UnitOfWork>>
    {
        public ActionResult Index()
        {
            var courses = Service.GetAllCourses();
            return View(courses);
        }
    }
}