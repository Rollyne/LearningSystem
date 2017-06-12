using System.Data.Entity;
using LearningSystem.Data.Common;
using LearningSystem.Data.Repositories;

namespace LearningSystem.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public UnitOfWork()
        {
            this.context = new LearningSystemContext();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
            => new Repository<TEntity>(this.context);

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
