using BabyStore.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BabyStore.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext(): base("StoreContext")
        {            
        }

        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductImageMapping> ProductImageMappings { get; set; }
        public DbSet<BasketLine> BasketLine { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}