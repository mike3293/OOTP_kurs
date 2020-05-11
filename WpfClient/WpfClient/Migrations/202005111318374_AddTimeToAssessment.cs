namespace WpfClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeToAssessment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assessments", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assessments", "Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
