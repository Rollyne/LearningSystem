namespace LearningSystem.Models.Mapper
{
    public static class AutoMapperModelsConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<ArticleProfile>();
                    cfg.AddProfile<CourseProfile>();
                    cfg.AddProfile<StudentCourseProfile>();
                    cfg.AddProfile<ApplicationUserProfile>();
                }
            );
        }
    }
}
