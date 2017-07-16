using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Services.Tools.Generic;

namespace LearningSystem.Services.Generic
{
    public interface IReadService<TDetailsViewModel, TIndexViewModel, TFilterViewModel, TEntity>
    {
        IExecutionResult<TDetailsViewModel> GetDetails(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TDetailsViewModel>> select = null);
        IExecutionResult<Tuple<List<TIndexViewModel>, int>> GetAllFiltered(TFilterViewModel filter);
    }
}
