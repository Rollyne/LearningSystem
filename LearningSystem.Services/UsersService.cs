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
    public class UsersService<TUnitOfWork> : CrudService< TUnitOfWork ,ApplicationUser, UserIndexViewModel, UserDetailsViewModel, UserFilterViewModel, ApplicationUser>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public UsersService() : base(new TUnitOfWork())
        {
        }

        protected override ApplicationUser ParseModifyViewModelToEntity(ApplicationUser model)
        {
            return model;
        }

        protected override Expression<Func<ApplicationUser, UserIndexViewModel>> SelectIndexViewModelQuery =>
            i => new UserIndexViewModel()
            {
                Id = i.Id,
                CUsername = i.CUsername,
                Name = i.Id
            };

        protected override Expression<Func<ApplicationUser, UserDetailsViewModel>> SelectDetailsViewModelQuery =>
            i => new UserDetailsViewModel()
            {
                Id = i.Id,
                Email = i.Email,
                CUsername = i.CUsername,
                Name = i.Name,
                BirthDate = i.BirthDate
            };

        protected override Expression<Func<ApplicationUser, ApplicationUser>> SelectModifyViewModelQuery =>
            i => i;
        public override IExecutionResult<Tuple<List<UserIndexViewModel>, int>> GetAllFiltered(UserFilterViewModel filter)
        {
            return base.getAllFiltered(filter, where: i => true, order: i => i.Id);
        }
    }
}
