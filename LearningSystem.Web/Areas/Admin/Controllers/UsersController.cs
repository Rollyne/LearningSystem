using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController 
        : ServiceController<UsersService<UnitOfWork>>
    {
    }
}
