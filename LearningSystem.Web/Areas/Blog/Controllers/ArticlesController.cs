using System.Web.Mvc;

namespace LearningSystem.Web.Areas.Blog.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        // GET: Blog/Articles
        public ActionResult Index()
        {
            return View();
        }

        // GET: Blog/Articles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "BlogAuthor")]
        // GET: Blog/Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Articles/Create
        [Authorize(Roles = "BlogAuthor")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Blog/Articles/Edit/5
        [Authorize(Roles = "BlogAuthor")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Blog/Articles/Edit/5
        [HttpPost]
        [Authorize(Roles = "BlogAuthor")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Blog/Articles/Delete/5
        [Authorize(Roles = "BlogAuthor")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Blog/Articles/Delete/5
        [HttpPost]
        [Authorize(Roles = "BlogAuthor")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
