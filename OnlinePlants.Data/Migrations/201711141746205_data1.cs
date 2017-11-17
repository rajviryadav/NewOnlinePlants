namespace OnlinePlants.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class data1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Registration", "CreatedBy");
            DropColumn("dbo.Registration", "UpdatedBy");
            DropColumn("dbo.User", "CreatedBy");
            DropColumn("dbo.User", "UpdatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "UpdatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.User", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Registration", "UpdatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Registration", "CreatedBy", c => c.Int(nullable: false));
        }
    }
}
