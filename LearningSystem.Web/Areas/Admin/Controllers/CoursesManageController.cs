using System;
using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;
using Microsoft.AspNet.Identity;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Authorize(Roles= "Administrator")]
    public class CoursesManageController : ServiceController<CoursesService<UnitOfWork>>
    {
        // GET: Admin/Courses
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

        // GET: Admin/Courses/Details/5
        public ActionResult Details(int id)
        {
            var execution = Service.GetDetails(id, HttpContext.User.Identity.GetUserId());

            if (!execution.Succeded)
                return HttpNotFound();

            return View(execution.Result);
        }

        // GET: Admin/Courses/Create
        public ActionResult Create()
        {
            return View(new CourseModifyViewModel());
        }

        // POST: Admin/Courses/Create
        [HttpPost]
        public ActionResult Create(CourseModifyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = Service.AddNewCourse(model);
                if (result.Succeded) return RedirectToAction("Index");

                ModelState.AddModelError("", result.Message);
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Courses/Edit/5
        public ActionResult Edit(int id)
        {
            var result = Service.GetByIdForModification(id);

            if (result.Succeded) return View(result.Result);

            if (result.Result == null)
                return HttpNotFound();

            ModelState.AddModelError("", result.Message);
            return View(result.Result);

        }

        // POST: Admin/Courses/Edit/5
        [HttpPost]
        public ActionResult Edit(CourseModifyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = Service.Update(model);

                if (result.Succeded) return RedirectToAction("Index");

                ModelState.AddModelError("", result.Message);
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Courses/Delete/5
        public ActionResult Delete(int id)
        {
            var result = Service.GetByIdForModification(id);

            if (result.Succeded) return View(result.Result);

            if (result.Result == null)
                return HttpNotFound();

            ModelState.AddModelError("", result.Message);
            return View(result.Result);
        }

        // POST: Admin/Courses/Delete/5
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                var result = Service.Delete(id);

                if(result.Succeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Delete", new {id});
            }
            catch
            {
                return RedirectToAction("Delete", new {id});
            }
        }
    }
}
