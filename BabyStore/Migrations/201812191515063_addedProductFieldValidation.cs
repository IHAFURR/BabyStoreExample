namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProductFieldValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Product", "Description", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Description", c => c.String());
            AlterColumn("dbo.Product", "Name", c => c.String());
        }
    }
}
