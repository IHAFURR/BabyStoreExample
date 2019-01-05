namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueFileName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductImage", "FileName", c => c.String(maxLength: 100));
            CreateIndex("dbo.ProductImage", "FileName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductImage", new[] { "FileName" });
            AlterColumn("dbo.ProductImage", "FileName", c => c.String());
        }
    }
}
