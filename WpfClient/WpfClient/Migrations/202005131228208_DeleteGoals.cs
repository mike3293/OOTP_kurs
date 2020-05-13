namespace WpfClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteGoals : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InternshipGoals", "Internship_Id", "dbo.Internships");
            DropIndex("dbo.InternshipGoals", new[] { "Internship_Id" });
            DropTable("dbo.InternshipGoals");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.InternshipGoals", "Internship_Id");
            AddForeignKey("dbo.InternshipGoals", "Internship_Id", "dbo.Internships", "Id");
        }
    }
}
