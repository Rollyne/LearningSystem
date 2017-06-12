using System.Data.Entity;
using LearningSystem.Models.EntityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LearningSystem.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class LearningSystemContext : IdentityDbContext<ApplicationUser>
    {
        public LearningSystemContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static LearningSystemContext Create()
        {
            return new LearningSystemContext();
        }

        public IDbSet<Student> Studetns { get; set; }

        public IDbSet<Article> Articles { get; set; }

        public IDbSet<Course> Courses { get; set; }

        public IDbSet<StudentsCourses> StudentsCourseses { get; set; }
    }
}