using System.Collections.Generic;
using LearningSystem.Data;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;

namespace LearningSystem.Services
{
    public class HomeService<TUnitOfWork> : Service<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public HomeService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public HomeService()
            :base(new TUnitOfWork())
        {
            
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return unitOfWork.GetRepository<Course>().GetAll();
        }
    }
}
