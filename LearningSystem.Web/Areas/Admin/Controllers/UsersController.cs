using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;
using Microsoft.AspNet.Identity.Owin;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController 
        : ServiceController<UsersService<UnitOfWork>>
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _UsersListPartial(UserFilterViewModel filter)
        {
            var execution = Service.GetAllFiltered(filter);

            if (!execution.Succeded)
                return HttpNotFound();

            var model = execution.Result.Item1;
            var itemsPerPage = filter.ItemsPerPage ?? ApplicationConstants.DefaultItemsPerPage;
            TempData["Pages"] = Math.Ceiling((double)execution.Result.Item2 / itemsPerPage);
            TempData["CurrentPage"] = filter.Page == 0 ? 1 : filter.Page;

            return PartialView("_UsersListPartial", model);
        }

        [HttpGet]
        public ActionResult _SearchPartial(CourseFilterViewModel model)
        {
            return PartialView("_SearchPartial", model ?? new CourseFilterViewModel());
        }

        [HttpGet]
        public ActionResult _PaginationPartial(CourseFilterViewModel model)
        {
            return PartialView("_PaginationPartial", model ?? new CourseFilterViewModel());
        }

        public async Task<ActionResult> Details(string id)
        {
            var execution = Service.GetDetails(id);
            if (!execution.Succeded)
            {
                if (execution.Result == null)
                    return HttpNotFound();

                AddError(execution.Message);
                return RedirectToAction("Details", new {id});
            }

            execution.Result.Roles = await UserManager.GetRolesAsync(id);

            return View(execution.Result);
        }
    }
}
