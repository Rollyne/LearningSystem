using System.Collections.Generic;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Article;

namespace LearningSystem.Services
{
    public class ArticlesService<TUnitOfWork> : Service<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork, new()
    {
        public ArticlesService(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ArticlesService()
            : base(new TUnitOfWork())
        {
        }

        public IEnumerable<ArticleIndexViewModel> GetTheNewest(int count)
        {
            return null;
        }

    }
}