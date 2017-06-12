using System;

namespace LearningSystem.Data.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        void Save();
    }
}
