using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Article;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;

namespace LearningSystem.Web.Areas.Blog.Controllers
{
    [Authorize]
    public class ArticlesController : ServiceController<ArticlesService<UnitOfWork>>
    {
        // GET: Blog/Articles
        public ActionResult Index()
        {
            var execution = Service.GetTheNewest(3);
            if (!execution.Succeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(execution.Result);
        }

        // GET: Blog/Articles/Details/5
        public ActionResult Details(int id)
        {
            var execution = Service.GetDetails(id);

            if (!execution.Succeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(execution.Result);
        }

        [Authorize(Roles = "BlogAuthor")]
        // GET: Blog/Articles/Create
        public ActionResult Create(ArticleModifyViewModel model)
        {
            var execution = Service.Create(model);
            if (!execution.Succeded)
            {
                return View();
            }
            return RedirectToAction("Index");
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
