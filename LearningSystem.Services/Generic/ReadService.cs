using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services.Tools.Generic;
using LearningSystem.Services.Tools.Messages;

namespace LearningSystem.Services.Generic
{
    public abstract class ReadService<TUnitOfWork, TDetailsViewModel, TIndexViewModel, TFilterViewModel, TEntity> 
        : Service<TUnitOfWork>,
        IReadService<TDetailsViewModel, TIndexViewModel, TFilterViewModel, TEntity> 
        where TUnitOfWork : IUnitOfWork 
        where TEntity : class
        where TFilterViewModel : IPagerFilter
    {
        protected ReadService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public abstract IExecutionResult<Tuple<List<TIndexViewModel>, int>> GetAllFiltered(TFilterViewModel filter);
        protected IExecutionResult<Tuple<List<TIndexViewModel>, int>> getAllFiltered<TKey>(TFilterViewModel filter, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> order)
        {
            var itemsAndPages = unitOfWork.GetRepository<TEntity>()
                .GetAllPaged<TKey, TIndexViewModel>(
                page: filter.Page,
                itemsPerPage: filter.ItemsPerPage ?? ApplicationConstants.DefaultItemsPerPage,
                where: where,
                orderBy: order);
            var result = new ExecutionResult<Tuple<List<TIndexViewModel>, int>>()
            {
                Result = itemsAndPages,
                Message = "",
                Succeded = true
            };
            return result;
        }

        public virtual IExecutionResult<TDetailsViewModel> GetDetails(Expression<Func<TEntity, bool>> @where,
            Expression<Func<TEntity, TDetailsViewModel>> @select = null)
            => this.getDetails(where, select);
        protected IExecutionResult<TDetailsViewModel> getDetails(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TDetailsViewModel>> select = null, object mapperParameters = null)
        {
            var result = new ExecutionResult<TDetailsViewModel>();

            TDetailsViewModel item = default(TDetailsViewModel);
            if (select == null)
            {
                item = unitOfWork.GetRepository<TEntity>()
                .FirstOrDefault<TDetailsViewModel>(
                    where: where,
                    mapperParameters: mapperParameters);
            }
            else
            {
                item = unitOfWork.GetRepository<TEntity>()
                .FirstOrDefault(
                    where: where,
                    select: select);
            }
            if (item == null)
            {
                result.Succeded = false;
                result.Message = CrudMessages.NotFound();
                result.Result = default(TDetailsViewModel);

                return result;
            }
            result.Result = item;
            result.Message = "";
            result.Succeded = true;

            return result;
        }
    }
}
