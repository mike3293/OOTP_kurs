namespace WpfClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assessments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Location = c.String(),
                        Topic = c.String(),
                        Internship_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Internships", t => t.Internship_Id, cascadeDelete: true)
                .Index(t => t.Internship_Id);
            
            CreateTable(
                "dbo.Internships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        Intern_Id = c.Int(nullable: false),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Intern_Id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.Manager_Id)
                .Index(t => t.Intern_Id)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InternshipGoals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeadlineDate = c.DateTime(nullable: false, storeType: "date"),
                        Name = c.String(nullable: false),
                        IsCompleted = c.Boolean(),
                        Internship_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Internships", t => t.Internship_Id)
                .Index(t => t.Internship_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        HashedPassword = c.String(nullable: false),
                        Role = c.Int(nullable: false),
                        UserDetails_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.UserDetails_Id, cascadeDelete: true)
                .Index(t => t.UserDetails_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserDetails_Id", "dbo.People");
            DropForeignKey("dbo.Assessments", "Internship_Id", "dbo.Internships");
            DropForeignKey("dbo.Internships", "Manager_Id", "dbo.People");
            DropForeignKey("dbo.InternshipGoals", "Internship_Id", "dbo.Internships");
            DropForeignKey("dbo.Internships", "Intern_Id", "dbo.People");
            DropIndex("dbo.Users", new[] { "UserDetails_Id" });
            DropIndex("dbo.InternshipGoals", new[] { "Internship_Id" });
            DropIndex("dbo.Internships", new[] { "Manager_Id" });
            DropIndex("dbo.Internships", new[] { "Intern_Id" });
            DropIndex("dbo.Assessments", new[] { "Internship_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.InternshipGoals");
            DropTable("dbo.People");
            DropTable("dbo.Internships");
            DropTable("dbo.Assessments");
        }
    }
}
