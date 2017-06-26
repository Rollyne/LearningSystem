using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services.Tools;
using LearningSystem.Services.Tools.Generic;
using LearningSystem.Services.Tools.Messages;

namespace LearningSystem.Services.Generic
{
    public abstract class CrudService<TUnitOfWork, TModifyViewModel, TIndexViewModel, TDetailsViewModel, TFilterViewModel, TEntity>
        : Service<TUnitOfWork>, ICrudService<TModifyViewModel, TIndexViewModel, TDetailsViewModel, TFilterViewModel, TEntity> 
        where TUnitOfWork : IUnitOfWork 
        where TEntity : class, new()
        where TFilterViewModel : IPagerFilter
    {
        public CrudService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected abstract TEntity ParseModifyViewModelToEntity(TModifyViewModel model);
        protected abstract Expression<Func<TEntity, TIndexViewModel>> SelectIndexViewModelQuery { get; }

        protected abstract Expression<Func<TEntity, TDetailsViewModel>> SelectDetailsViewModelQuery { get; }

        protected abstract Expression<Func<TEntity, TModifyViewModel>> SelectModifyViewModelQuery { get; }

        public IExecutionResult Create(TModifyViewModel model)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var item = ParseModifyViewModelToEntity(model);

            repo.Add(item);
            unitOfWork.Save();

            return new ExecutionResult()
            {
                Succeded = true,
                Message = CrudMessages.SuccessfulCreationOf()
            };
        }

        public IExecutionResult Delete(TEntity item)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            repo.Delete(item);
            unitOfWork.Save();

            return new ExecutionResult()
            {
                Succeded = true,
                Message = CrudMessages.SuccessfulCreationOf()
            };
        }

        public abstract IExecutionResult<Tuple<List<TIndexViewModel>, int>> GetAllFiltered(TFilterViewModel filter);
        protected IExecutionResult<Tuple<List<TIndexViewModel>, int>> getAllFiltered<TKey>(TFilterViewModel filter, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> order)
        {
            var itemsAndPages = unitOfWork.GetRepository<TEntity>()
                .GetAllPaged(
                page: filter.Page,
                itemsPerPage: filter.ItemsPerPage == 0 ? ApplicationConstants.DefaultItemsPerPage : filter.ItemsPerPage,
                where: where,
                select: SelectIndexViewModelQuery,
                orderBy: order);
            var result = new ExecutionResult<Tuple<List<TIndexViewModel>, int>>()
            {
                Result = itemsAndPages,
                Message = "",
                Succeded = true
            };
            return result;
        }

        public IExecutionResult<TModifyViewModel> getForModification(Expression<Func<TEntity, bool>> where)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var result = new ExecutionResult<TModifyViewModel>
            {
                Succeded = false
            };

            var item = repo.FirstOrDefault(where: where,
                select: SelectModifyViewModelQuery);
            if (item == null)
            {
                result.Message = CrudMessages.NotFound();
                result.Result = default(TModifyViewModel);
                return result;
            }

            result.Succeded = true;
            result.Result = item;

            return result;
        }
        
        protected IExecutionResult<TDetailsViewModel> getDetails(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TDetailsViewModel>> select = null)
        {
            var result = new ExecutionResult<TDetailsViewModel>();

            var item = unitOfWork.GetRepository<TEntity>()
                .FirstOrDefault(
                    where: where,
                    select: select ?? SelectDetailsViewModelQuery);
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

        public IExecutionResult Update(TModifyViewModel model)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var item = ParseModifyViewModelToEntity(model);

            repo.Update(item);
            unitOfWork.Save();

            return new ExecutionResult()
            {
                Succeded = true,
                Message = CrudMessages.SuccessfulCreationOf()
            };
        }

        public virtual IExecutionResult<TModifyViewModel> GetForModification(Expression<Func<TEntity, bool>> @where)
            => this.getForModification(where);

        public virtual IExecutionResult<TDetailsViewModel> GetDetails(Expression<Func<TEntity, bool>> @where,
            Expression<Func<TEntity, TDetailsViewModel>> @select = null)
            => this.getDetails(where, select);

        public IExecutionResult Delete(Expression<Func<TEntity, bool>> @where)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var item = repo.FirstOrDefault(where: where.Compile());

            return this.Delete(item);
        }
    }
}
