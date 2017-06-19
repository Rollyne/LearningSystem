using System.Collections.Generic;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Article;
using LearningSystem.Services.Tools.Generic;

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

        public IExecutionResult<IEnumerable<ArticleIndexViewModel>> GetTheNewest(int count)
        {
            var repo = unitOfWork.GetRepository<Article>();
            var result = repo.GetAll(
                take:count,
                orderByKeySelector: a => a.PublishDate,
                descending: true,
                select: a => new ArticleIndexViewModel()
                {
                    Id = a.Id,
                    Title = a.Title,
                    PublishDate = a.PublishDate,
                    AuthorName = a.Author.Name
                });

            var execution = new ExecutionResult<IEnumerable<ArticleIndexViewModel>>()
            {
                Succeded = true,
                Message = "",
                Result = result
            };

            return execution;
        }

    }
}