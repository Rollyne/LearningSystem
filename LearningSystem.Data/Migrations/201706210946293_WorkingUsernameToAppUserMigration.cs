namespace LearningSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkingUsernameToAppUserMigration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Courses", name: "Trainer_Id", newName: "TrainerId");
            RenameIndex(table: "dbo.Courses", name: "IX_Trainer_Id", newName: "IX_TrainerId");
            AddColumn("dbo.AspNetUsers", "CUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CUsername");
            RenameIndex(table: "dbo.Courses", name: "IX_TrainerId", newName: "IX_Trainer_Id");
            RenameColumn(table: "dbo.Courses", name: "TrainerId", newName: "Trainer_Id");
        }
    }
}
