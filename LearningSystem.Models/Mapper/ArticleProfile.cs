using AutoMapper;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Article;

namespace LearningSystem.Models.Mapper
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDetailsViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
            CreateMap<Article, ArticleIndexViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));

            CreateMap<ArticleModifyViewModel, Article>();
        }
    }
}
