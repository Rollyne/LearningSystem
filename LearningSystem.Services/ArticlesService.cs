using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Article;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services.Generic;
using LearningSystem.Services.Tools;
using LearningSystem.Services.Tools.Generic;
using LearningSystem.Services.Tools.Messages;

namespace LearningSystem.Services
{
    public class ArticlesService<TUnitOfWork> : CrudService<TUnitOfWork, ArticleModifyViewModel, ArticleIndexViewModel, ArticleDetailsViewModel, ArticleFilterViewModel, Article>
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

        public IExecutionResult<ArticleDetailsViewModel> GetDetails(int id)
        {
            return base.getDetails(where: i => i.Id == id, select: i => new ArticleDetailsViewModel()
            {
                Title = i.Title,
                Id = i.Id,
                AuthorName = i.Author.Name,
                Content = i.Content,
                PublishDate = i.PublishDate
            });
        }

        public override IExecutionResult Create(ArticleModifyViewModel model)
        {
            model.PublishDate = DateTime.Now;

            return base.Create(model);
        }

        public IExecutionResult Update(ArticleModifyViewModel model, string currentLoggedUserId)
        {
            if (model.AuthorId == currentLoggedUserId)
            {
                return base.Update(model);
            }

            return new ExecutionResult()
            {
                Succeded = false,
                Message = GlobalMessages.NoAccess("article")
            };
        }

        public IExecutionResult Delete(int id, string currentLoggedUserId)
        {
            var repo = unitOfWork.GetRepository<Article>();
            var item = repo.FirstOrDefault(i => i.Id == id);

            if (item.AuthorId == currentLoggedUserId)
            {
                return base.Delete(item);
            }

            return new ExecutionResult()
            {
                Succeded = false,
                Message = GlobalMessages.NoAccess("article")
            };
        }

        public override IExecutionResult<Tuple<List<ArticleIndexViewModel>, int>> GetAllFiltered(ArticleFilterViewModel filter)
        {
            return base.getAllFiltered(filter: filter, where: i => true, order: i => i.Id);
        }
    }
}