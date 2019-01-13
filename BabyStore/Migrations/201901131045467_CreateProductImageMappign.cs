namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductImageMappign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImageMapping",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageNumber = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        ProductImageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.ProductImage", t => t.ProductImageID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ProductImageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImageMapping", "ProductImageID", "dbo.ProductImage");
            DropForeignKey("dbo.ProductImageMapping", "ProductID", "dbo.Product");
            DropIndex("dbo.ProductImageMapping", new[] { "ProductImageID" });
            DropIndex("dbo.ProductImageMapping", new[] { "ProductID" });
            DropTable("dbo.ProductImageMapping");
        }
    }
}
