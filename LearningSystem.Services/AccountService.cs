using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;

namespace LearningSystem.Services
{
    public class AccountService<TUnitOfWork> : Service<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public AccountService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public AccountService()
            : base(new TUnitOfWork())
        {
        }

        public void AddUserToStudent(string userId)
        {
            var repo = unitOfWork.GetRepository<Student>();

            if (repo.Any(i => i.Id == userId))
                return;

            repo.Add(new Student() { Id = userId });
            unitOfWork.Save();
        }
    }
}
