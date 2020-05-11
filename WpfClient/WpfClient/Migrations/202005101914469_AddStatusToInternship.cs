namespace WpfClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToInternship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "IsCompleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "IsCompleted");
        }
    }
}
