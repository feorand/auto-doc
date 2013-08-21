namespace AutoDoc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserLogin = c.String(nullable: false, maxLength: 20),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 6),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.ControlForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 6),
                        Description = c.String(nullable: false, maxLength: 50),
                        Week = c.Int(nullable: false),
                        MaxScore = c.Int(nullable: false),
                        SectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        ControlForm_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlForms", t => t.ControlForm_Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ControlForm_Id)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(nullable: false, maxLength: 50),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.SubjectUsers",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.User_UserId })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SubjectUsers", new[] { "User_UserId" });
            DropIndex("dbo.SubjectUsers", new[] { "Subject_Id" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Marks", new[] { "StudentId" });
            DropIndex("dbo.Marks", new[] { "ControlForm_Id" });
            DropIndex("dbo.ControlForms", new[] { "SectionId" });
            DropIndex("dbo.Sections", new[] { "SubjectId" });
            DropIndex("dbo.Subjects", new[] { "GroupId" });
            DropForeignKey("dbo.SubjectUsers", "User_UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.SubjectUsers", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Marks", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Marks", "ControlForm_Id", "dbo.ControlForms");
            DropForeignKey("dbo.ControlForms", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Sections", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "GroupId", "dbo.Groups");
            DropTable("dbo.SubjectUsers");
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
            DropTable("dbo.Marks");
            DropTable("dbo.ControlForms");
            DropTable("dbo.Sections");
            DropTable("dbo.Subjects");
            DropTable("dbo.UserProfiles");
        }
    }
}
