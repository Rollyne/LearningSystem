using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Models.ViewModels.User;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController 
        : CrudController<UsersService<UnitOfWork>, ApplicationUser, UserFilterViewModel, UserIndexViewModel, UserDetailsViewModel, ApplicationUser>
    {
        [HttpGet]
        public ActionResult Details(string id)
        {
            return base.details(c => c.Id == id);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return base.edit(c => c.Id == id);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            return base.delete(c => c.Id == id);
        }

        [HttpPost]
        public ActionResult ConfirmDelete(string id, string returnUrl)
        {
            return base.confirmDelete(c => c.Id == id, returnUrl);
        }
    }
}
