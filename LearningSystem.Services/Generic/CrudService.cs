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

        public virtual IExecutionResult Create(TModifyViewModel model)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var item = AutoMapper.Mapper.Map<TModifyViewModel, TEntity>(model);

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

        public IExecutionResult<TModifyViewModel> getForModification(Expression<Func<TEntity, bool>> where)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var result = new ExecutionResult<TModifyViewModel>
            {
                Succeded = false
            };

            var item = repo.FirstOrDefault<TModifyViewModel>(where: where);
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

        public virtual IExecutionResult Update(TModifyViewModel model)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var item = AutoMapper.Mapper.Map<TModifyViewModel, TEntity>(model);

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
