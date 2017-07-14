using AutoMapper;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.StudentsCourses;

namespace LearningSystem.Models.Mapper
{
    public class StudentCourseProfile : Profile
    {
        public StudentCourseProfile()
        {
            CreateMap<StudentsCourses, GradeStudentViewModel>();
        }
    }
}
