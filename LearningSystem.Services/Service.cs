using LearningSystem.Data.Common;

namespace LearningSystem.Services
{
    public abstract class Service<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork, new()
    {
        protected TUnitOfWork unitOfWork;

        protected Service(TUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
