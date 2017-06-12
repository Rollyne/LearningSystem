namespace LearningSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialEntitiesAndRelationshipsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Trainer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Trainer_Id)
                .Index(t => t.Trainer_Id);
            
            CreateTable(
                "dbo.StudentsCourses",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Trainer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentsCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentsCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Articles", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Students", new[] { "Id" });
            DropIndex("dbo.StudentsCourses", new[] { "CourseId" });
            DropIndex("dbo.StudentsCourses", new[] { "StudentId" });
            DropIndex("dbo.Courses", new[] { "Trainer_Id" });
            DropIndex("dbo.Articles", new[] { "Author_Id" });
            DropColumn("dbo.AspNetUsers", "BirthDate");
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.Students");
            DropTable("dbo.StudentsCourses");
            DropTable("dbo.Courses");
            DropTable("dbo.Articles");
        }
    }
}
