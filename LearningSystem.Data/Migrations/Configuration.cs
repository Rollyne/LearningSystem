using LearningSystem.Models.EntityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LearningSystem.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LearningSystem.Data.LearningSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LearningSystemContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            foreach (var roleName in Constants.Roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == roleName))
                {
                    var role = new IdentityRole(roleName);
                    roleManager.Create(role);
                }
            }

            context.Courses.Add(new Course()
            {
                Name = "Probaaa1",
                Description = "Description1",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(15)
            });
            context.Courses.Add(new Course()
            {
                Name = "Probaaa2",
                Description = "Description1",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(15)
            });
            context.Courses.Add(new Course()
            {
                Name = "Probaaa3",
                Description = "Description1",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(15)
            });
            context.Courses.Add(new Course()
            {
                Name = "Probaaa4",
                Description = "Description1",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(15)
            });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
