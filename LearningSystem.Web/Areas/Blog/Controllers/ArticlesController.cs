using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Article;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;
using Microsoft.AspNet.Identity;

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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Articles/Create
        [Authorize(Roles = "BlogAuthor")]
        [HttpPost]
        public ActionResult Create(ArticleModifyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                model.AuthorId = HttpContext.User.Identity.GetUserId();
                var execution = Service.Create(model);
                if (!execution.Succeded)
                {
                    return View();
                }

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
        public ActionResult Edit(ArticleModifyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                Service.Update(model, HttpContext.User.Identity.GetUserId());

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Blog/Articles/Delete/5
        [HttpPost]
        [Authorize(Roles = "BlogAuthor")]
        public ActionResult Delete(int id)
        {
            try
            {
                Service.Delete(id, HttpContext.User.Identity.GetUserId());

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
