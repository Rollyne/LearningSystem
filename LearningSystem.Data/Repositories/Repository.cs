using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using LearningSystem.Data.Common;

namespace LearningSystem.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected DbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        public void Add(TEntity item)
        {
            Context.Set<TEntity>().Add(item);
        }

        public void Update(TEntity item)
        {
            Context.Set<TEntity>().AddOrUpdate(item);
        }

        public void Delete(TEntity item)
        {
            Context.Set<TEntity>().Remove(item);
        }

        public ICollection<TEntity> Where(Func<TEntity, bool> condition)
        {
            return Context.Set<TEntity>().Where(condition).ToList();
        }

        public int Count()
        {
            return Context.Set<TEntity>().Count();
        }

        private IQueryable<TEntity> _getAllWhere(Expression<Func<TEntity, bool>> @where)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }

            return result;
        }

        private IQueryable<TEntity> _getAllWhereAndOrder<TKey>(
            Expression<Func<TEntity, bool>> @where,
            Expression<Func<TEntity, TKey>> orderBy,
            bool descending
            )
        {
            IQueryable<TEntity> result = _getAllWhere(where);
            if (orderBy != null)
            {
                result = descending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
            }

            return result;
        }

        public Tuple<List<TEntity>, int> GetAllPaged<TKey>(
            int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderBy = null,
            bool descending = false)
        {
            IQueryable<TEntity> result = _getAllWhereAndOrder(where, orderBy, descending);

            if (itemsPerPage != 0 && page != 0)
            {
                var skip = (page - 1) * itemsPerPage;
                return Tuple.Create(result.Skip(skip).Take(itemsPerPage).ToList(), result.Count());
            }

            return Tuple.Create(result.ToList(), result.Count());
        }

        public Tuple<List<TResult>, int> GetAllPaged<TKey, TResult>(
            Expression<Func<TEntity, TResult>> @select,
            int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderBy = null,
            bool descending = false)
        {
            IQueryable<TEntity> result = _getAllWhereAndOrder(where, orderBy, descending);

            if (itemsPerPage != 0 && page != 0)
            {
                var skip = (page - 1) * itemsPerPage;
                return Tuple.Create(result.Skip(skip).Take(itemsPerPage).Select(select).ToList(), result.Count());
            }

            return Tuple.Create(result.Select(select).ToList(), result.Count());
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> @where)
        {
            return Context.Set<TEntity>().FirstOrDefault(where);
        }

        public TResult FirstOrDefault<TResult>(Expression<Func<TEntity, bool>> @where,
            Expression<Func<TEntity, TResult>> @select)
        {
            return Context.Set<TEntity>().Where(where).Select(select).FirstOrDefault();
        }

        public Tuple<List<TEntity>, int> GetAllPaged(int itemsPerPage = 0, int page = 0,
            Expression<Func<TEntity, bool>> @where = null)
        {
            IQueryable<TEntity> result = _getAllWhere(where);

            if (itemsPerPage != 0 && page != 0)
                return Tuple.Create(result.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList(), result.Count());
            return Tuple.Create(result.ToList(), result.Count());
        }

        public ICollection<TEntity> GetAll(int take = -1)
        {
            return take < 0 ? Context.Set<TEntity>().ToList() : Context.Set<TEntity>().Take(take).ToList();
        }

        public ICollection<TEntity> GetAll<TKey>(Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null, bool @descending = false, int take = -1)
        {
            IQueryable<TEntity> result = _getAllWhereAndOrder(where, orderByKeySelector, descending);

            return take < 0 ? Context.Set<TEntity>().ToList() : Context.Set<TEntity>().Take(take).ToList();
        }

        public ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> @where, int take = -1)
        {
            IQueryable<TEntity> result = _getAllWhere(where);

            return take < 0 ? Context.Set<TEntity>().ToList() : Context.Set<TEntity>().Take(take).ToList();
        }

        public ICollection<TResult> GetAll<TKey, TResult>(Expression<Func<TEntity, TResult>> @select,
            Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool @descending = false, int take = -1)
        {
            IQueryable<TEntity> result = _getAllWhereAndOrder(where, orderByKeySelector, descending);

            return take < 0 ? Context.Set<TEntity>().Select(select).ToList() : Context.Set<TEntity>().Select(select).Take(take).ToList();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public ICollection<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> @select, Expression<Func<TEntity, bool>> @where = null, int take = -1)
        {
            IQueryable<TEntity> result = _getAllWhere(where);

            return take < 0 ? Context.Set<TEntity>().Select(select).ToList() : Context.Set<TEntity>().Select(select).Take(take).ToList();
        }

        public bool Any(Expression<Func<TEntity, bool>> any)
        {
            return Context.Set<TEntity>().Any(any);
        }

        public ICollection<TResult> GetAll<TKey, TResult>(Expression<Func<TEntity, bool>> @where = null, Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool @descending = false, int take = -1)
        {
            IQueryable<TEntity> result = _getAllWhereAndOrder(where, orderByKeySelector, descending);

            return take < 0 ? Context.Set<TEntity>().ProjectTo<TResult>().ToList() : Context.Set<TEntity>().ProjectTo<TResult>().Take(take).ToList();
        }

        public ICollection<TResult> GetAll<TResult>(Expression<Func<TEntity, bool>> @where = null, int take = -1)
        {
            IQueryable<TEntity> result = _getAllWhere(where);

            return take < 0 ? Context.Set<TEntity>().ProjectTo<TResult>().ToList() : Context.Set<TEntity>().ProjectTo<TResult>().Take(take).ToList();
        }

        public TResult FirstOrDefault<TResult>(Expression<Func<TEntity, bool>> @where)
        {
            return Context.Set<TEntity>().Where(where).ProjectTo<TResult>().FirstOrDefault();
        }

        public Tuple<List<TResult>, int> GetAllPaged<TKey, TResult>(int itemsPerPage = 0, int page = 0, Expression<Func<TEntity, bool>> @where = null, Expression<Func<TEntity, TKey>> orderBy = null,
            bool @descending = false)
        {
            IQueryable<TEntity> result = _getAllWhereAndOrder(where, orderBy, descending);

            if (itemsPerPage != 0 && page != 0)
            {
                var skip = (page - 1) * itemsPerPage;
                return Tuple.Create(result.Skip(skip).Take(itemsPerPage).ProjectTo<TResult>().ToList(), result.Count());
            }

            return Tuple.Create(result.ProjectTo<TResult>().ToList(), result.Count());
        }
    }
}

