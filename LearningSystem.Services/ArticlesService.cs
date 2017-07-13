using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearningSystem.Data.Common;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Article;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services.Generic;
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

        protected override Article ParseModifyViewModelToEntity(ArticleModifyViewModel model)
        {
            return new Article()
            {
                Content = model.Content,
                Id = model.Id.Value,
                Title = model.Title
            };
        }

        protected override Expression<Func<Article, ArticleIndexViewModel>> SelectIndexViewModelQuery =>
            i => new ArticleIndexViewModel()
            {
                Id = i.Id,
                PublishDate = i.PublishDate,
                AuthorName = i.Author.Name,
                Title = i.Title
            };

        protected override Expression<Func<Article, ArticleDetailsViewModel>> SelectDetailsViewModelQuery =>
            i => new ArticleDetailsViewModel()
            {
                Id = i.Id,
                AuthorName = i.Author.Name,
                Content = i.Content,
                PublishDate = i.PublishDate,
                Title = i.Title
            };
        protected override Expression<Func<Article, ArticleModifyViewModel>> SelectModifyViewModelQuery =>
            i => new ArticleModifyViewModel()
            {
                Id = i.Id,
                Content = i.Content,
                Title = i.Title
            };
        public override IExecutionResult<Tuple<List<ArticleIndexViewModel>, int>> GetAllFiltered(ArticleFilterViewModel filter)
        {
            return base.getAllFiltered(filter: filter, where: i => true, order: i => i.Id);
        }
    }
}