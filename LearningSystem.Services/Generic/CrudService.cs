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
        : ReadService<TUnitOfWork, TDetailsViewModel, TIndexViewModel, TFilterViewModel, TEntity>,
        ICrudService<TModifyViewModel, TIndexViewModel, TDetailsViewModel, TFilterViewModel, TEntity> 
        where TUnitOfWork : IUnitOfWork 
        where TEntity : class, new()
        where TFilterViewModel : IPagerFilter
    {
        protected CrudService(TUnitOfWork unitOfWork) : base(unitOfWork)
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

        public IExecutionResult Delete(Expression<Func<TEntity, bool>> @where)
        {
            var repo = unitOfWork.GetRepository<TEntity>();

            var item = repo.FirstOrDefault(where: where.Compile());

            return this.Delete(item);
        }

        public IExecutionResult<TIndexViewModel> GetDetails(Expression<Func<TEntity, bool>> @where, Expression<Func<TEntity, TIndexViewModel>> @select = null)
        {
            throw new NotImplementedException();
        }
        
    }
}
