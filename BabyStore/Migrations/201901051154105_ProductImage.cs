namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductImage");
        }
    }
}
