using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services;
using LearningSystem.Services.Generic;

namespace LearningSystem.Web.Controllers.Generic
{
    public abstract class CrudController<TService, TModifyViewModel, TFilterViewModel, TIndexViewModel, TDetailsViewModel, TEntity> : 
        ServiceController<TService> 
        where TService :  ICrudService<TModifyViewModel, TIndexViewModel, TDetailsViewModel, TFilterViewModel, TEntity>, new()
        where TModifyViewModel : new()
        where TFilterViewModel : IPagerFilter

    {

        public ActionResult Index(TFilterViewModel filter)
        {
            var execution = Service.GetAllFiltered(filter);

            if (!execution.Succeded)
                return HttpNotFound();

            var model = execution.Result.Item1;
            var itemsPerPage = filter.ItemsPerPage == 0 ? ApplicationConstants.DefaultItemsPerPage : filter.ItemsPerPage;
            ViewBag.Pages = Math.Ceiling((double)execution.Result.Item2 / itemsPerPage);
            ViewBag.CurrentPage = filter.Page == 0 ? 1 : filter.Page;

            return View(model);
        }
        
        protected ActionResult details(Expression<Func<TEntity, bool>> getByIdQuery)
        {
            var execution = Service.GetDetails(getByIdQuery);

            if (!execution.Succeded)
                return HttpNotFound();

            return View(execution.Result);
        }
        
        public ActionResult Create()
        {
            return View(new TModifyViewModel());
        }
        
        [HttpPost]
        public ActionResult Create(TModifyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = Service.Create(model);
                if (result.Succeded) return RedirectToAction("Index");

                ModelState.AddModelError("", result.Message);
                return View(model);
            }
            catch
            {
                return View();
            }
        }
        
        protected ActionResult edit(Expression<Func<TEntity, bool>> getByIdQuery)
        {
            var result = Service.GetForModification(getByIdQuery);

            if (result.Succeded) return View(result.Result);

            if (result.Result == null)
                return HttpNotFound();

            ModelState.AddModelError("", result.Message);
            return View(result.Result);

        }
        
        [HttpPost]
        public ActionResult Edit(TModifyViewModel model)
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
            catch(DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                ModelState.AddModelError("", exceptionMessage);
                return View(model);
            }
        }
        
        protected ActionResult delete(Expression<Func<TEntity, bool>> getByIdQuery)
        {
            var result = Service.GetForModification(getByIdQuery);

            if (result.Succeded) return View(result.Result);

            if (result.Result == null)
                return HttpNotFound();

            ModelState.AddModelError("", result.Message);
            return View(result.Result);
        }

        // POST: Admin/Courses/Delete/5
        [HttpPost]
        protected ActionResult confirmDelete(Expression<Func<TEntity, bool>> getByIdQuery, string returnUrl)
        {
            try
            {
                var result = Service.Delete(getByIdQuery);

                if (result.Succeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", result.Message);
                return Redirect(returnUrl);
            }
            catch
            {
                ModelState.AddModelError("", "You cannot delete this user.");
                return Redirect(returnUrl);
            }
        }
    }
}
