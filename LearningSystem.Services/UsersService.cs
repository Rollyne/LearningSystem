using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Models.ViewModels.User;
using LearningSystem.Services.Generic;
using LearningSystem.Services.Tools.Generic;

namespace LearningSystem.Services
{
    public class UsersService<TUnitOfWork> : ReadService<TUnitOfWork, UserDetailsViewModel, UserIndexViewModel, UserFilterViewModel, ApplicationUser>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public UsersService() : base(new TUnitOfWork())
        {
        }

        public IExecutionResult<UserDetailsViewModel> GetDetails(string id)
        {
            return base.getDetails(where: u => u.Id == id);
        }

        public override IExecutionResult<Tuple<List<UserIndexViewModel>, int>> GetAllFiltered(UserFilterViewModel filter)
        {
            Expression<Func<ApplicationUser, bool>> where = course => true;
            if (!string.IsNullOrEmpty(filter.Search))
            {
                //TODO: Search logic
            }
            return base.getAllFiltered(where: where, filter: filter, order: c => c.Id);
        }
    }
}
