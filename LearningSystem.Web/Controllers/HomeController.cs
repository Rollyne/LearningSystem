using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Services;

namespace LearningSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService<UnitOfWork> service;
        public HomeController(HomeService<UnitOfWork> service)
        {
            this.service = service;
        }

        public HomeController()
        {
            this.service = new HomeService<UnitOfWork>();
        }

        public ActionResult Index()
        {
            var courses = service.GetAllCourses();
            return View(courses);
        }
    }
}