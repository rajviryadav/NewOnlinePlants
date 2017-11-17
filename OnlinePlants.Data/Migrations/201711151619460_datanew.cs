namespace OnlinePlants.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datanew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RegID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentType = c.Long(nullable: false),
                        TransactionID = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Registration",
                c => new
                    {
                        RegID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        ProfileURL = c.String(),
                        UserTypeID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RegID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        RegID = c.Int(nullable: false),
                        UserTypeID = c.Int(nullable: false),
                        Username = c.String(),
                        UserPassword = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserType");
            DropTable("dbo.User");
            DropTable("dbo.Registration");
            DropTable("dbo.Payments");
            DropTable("dbo.Category");
        }
    }
}
