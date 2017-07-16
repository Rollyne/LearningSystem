using AutoMapper;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.User;

namespace LearningSystem.Models.Mapper
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, UserDetailsViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
            CreateMap<ApplicationUser, UserIndexViewModel>();

        }
    }
}
