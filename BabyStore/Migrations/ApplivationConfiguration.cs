namespace BabyStore.Migrations.ApplicationConfiguration
{    
    using System.Data.Entity.Migrations;    

    internal sealed class ApplicationConfiguration : DbMigrationsConfiguration<BabyStore.Models.ApplicationDbContext>
    {
        public ApplicationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BabyStore.Model.ApplicationContext";
            MigrationsDirectory = @"Migrations\Application";
        }        
    }
}
