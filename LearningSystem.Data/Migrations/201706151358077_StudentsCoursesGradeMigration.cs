namespace LearningSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsCoursesGradeMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsCourses", "Grade", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsCourses", "Grade");
        }
    }
}
