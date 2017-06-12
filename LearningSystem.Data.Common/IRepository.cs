﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LearningSystem.Data.Common
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        void Add(TEntity item);

        ICollection<TEntity> GetAll();

        Tuple<List<TEntity>, int> GetAllPaged(int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> where = null);

        Tuple<List<TEntity>, int> GetAllPaged<TKey>(
            int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool descending = false);

        Tuple<List<TResult>, int> GetAllPaged<TKey, TResult>(
            int itemsPerPage = 0,
            int page = 0,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool descending = false,
            Expression<Func<TEntity, TResult>> select = null);

        ICollection<TEntity> GetAll<TKey>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool descending = false);

        ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> where);

        ICollection<TResult> GetAll<TKey, TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TKey>> orderByKeySelector = null,
            bool descending = false,
            Expression<Func<TEntity, TResult>> select = null);

        int Count();

        void Update(TEntity item);

        void Delete(TEntity item);

        TEntity FirstOrDefault(Func<TEntity, bool> where);

        TResult FirstOrDefault<TResult>(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TResult>> select = null);
    }
}