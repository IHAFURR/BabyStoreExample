
namespace BabyStore
{
    public static class Constants
    {
        public const string ProductImagePath = "~/Content/ProductImages/";
        public const string ProductThumbnailPath = "~/Content/ProductImages/Thumbnails/";
        public const int PageItems = 3;
        public const int NumberOfProdcutImages = 5;
        public const string UserRole = "Users";
    }

    public class Administrator
    {
        public readonly string Name;
        public readonly string Passord;
        // read from the file or ENVIROMENT
        public Administrator()
        {
            this.Name = "admin@test.com";
            this.Passord = "Adm1n@ihafurrePass3ord";
        }
    }
}