using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
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

        public Tuple<List<TEntity>, int> GetAllPaged<TKey>(
            int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool descending = false)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }
            if (orderByKeySelector != null)
            {
                result = descending ? result.OrderByDescending(orderByKeySelector) : result.OrderBy(orderByKeySelector);
            }
            if (itemsPerPage != 0 && page != 0)
            {
                var skip = (page - 1) * itemsPerPage;
                return Tuple.Create(result.Skip(skip).Take(itemsPerPage).ToList(), result.Count());
            }

            return Tuple.Create(result.ToList(), result.Count());
        }

        public Tuple<List<TResult>, int> GetAllPaged<TKey, TResult>(
            int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool descending = false,
            Expression<Func<TEntity, TResult>> @select = null)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }
            if (orderByKeySelector != null)
            {
                result = descending ? result.OrderByDescending(orderByKeySelector) : result.OrderBy(orderByKeySelector);
            }

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
            Expression<Func<TEntity, TResult>> @select = null)
        {
            return Context.Set<TEntity>().Where(where).Select(select).FirstOrDefault();
        }

        public Tuple<List<TEntity>, int> GetAllPaged(int itemsPerPage = 0, int page = 0,
            Expression<Func<TEntity, bool>> @where = null)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }
            if (itemsPerPage != 0 && page != 0)
                return Tuple.Create(result.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList(), result.Count());
            return Tuple.Create(result.ToList(), result.Count());
        }

        public ICollection<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public ICollection<TEntity> GetAll<TKey>(Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null, bool @descending = false)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }
            if (orderByKeySelector != null)
            {
                result = descending ? result.OrderByDescending(orderByKeySelector) : result.OrderBy(orderByKeySelector);
            }

            return result.ToList();
        }

        public ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> @where)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }
            return result.ToList();
        }

        public ICollection<TResult> GetAll<TKey, TResult>(Expression<Func<TEntity, bool>> @where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool @descending = false, Expression<Func<TEntity, TResult>> @select = null)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }
            if (orderByKeySelector != null)
            {
                result = descending ? result.OrderByDescending(orderByKeySelector) : result.OrderBy(orderByKeySelector);
            }

            return result.Select(select).ToList();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public Tuple<List<TResult>, int> GetAllPaged<TResult>(int itemsPerPage = 0, int page = 0, Expression<Func<TEntity, bool>> @where = null, bool @descending = false,
            Expression<Func<TEntity, TResult>> @select = null)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }

            if (itemsPerPage != 0 && page != 0)
            {
                var skip = (page - 1) * itemsPerPage;
                return Tuple.Create(result.Skip(skip).Take(itemsPerPage).Select(select).ToList(), result.Count());
            }

            return Tuple.Create(result.Select(select).ToList(), result.Count());
        }

        public ICollection<TResult> GetAll<TResult>(Expression<Func<TEntity, bool>> @where = null, bool @descending = false, Expression<Func<TEntity, TResult>> @select = null)
        {
            IQueryable<TEntity> result = Context.Set<TEntity>();

            if (where != null)
            {
                result = result.Where(where);
            }

            return result.Select(select).ToList();
        }
    }
}

