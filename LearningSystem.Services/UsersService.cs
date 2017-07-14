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
    public class UsersService<TUnitOfWork> : Service<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public UsersService() : base(new TUnitOfWork())
        {
        }
    }
}
