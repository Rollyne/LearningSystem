using System.Linq;
using AutoMapper;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Course;

namespace LearningSystem.Models.Mapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            string currentUserId = null;

            CreateMap<Course, CourseAdminDetailsViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.StudentsRelationships.Count));

            CreateMap<Course, CourseDetailsViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.StudentsRelationships.Count))
                .ForMember(dest => dest.IsCurrentUserSignedUp, 
                opt => opt.MapFrom(src => src.StudentsRelationships.Any(r => r.StudentId == currentUserId)))
                .ForMember(dest => dest.IsCurrentUserTrainer, opt => opt.MapFrom(src => src.TrainerId == currentUserId));

            CreateMap<Course, CourseIndexViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name));

            CreateMap<CourseModifyViewModel, Course>();
            CreateMap<Course, CourseModifyViewModel>();
        }
    }
}
