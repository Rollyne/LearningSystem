using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Services.Tools;
using LearningSystem.Services.Tools.Generic;

namespace LearningSystem.Services.Generic
{
    public interface ICrudService<TModifyViewModel, TIndexViewModel, TDetailsViewModel, TFilterViewModel, TEntity>
    {
        IExecutionResult Create(TModifyViewModel model);
        IExecutionResult Delete(TEntity item);
        IExecutionResult Delete(Expression<Func<TEntity, bool>> where);
        IExecutionResult<Tuple<List<TIndexViewModel>, int>> GetAllFiltered(TFilterViewModel filter);
        IExecutionResult<TModifyViewModel> GetForModification(Expression<Func<TEntity, bool>> where);
        IExecutionResult<TDetailsViewModel> GetDetails(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TDetailsViewModel>> select = null);
        IExecutionResult Update(TModifyViewModel model);
    }
}