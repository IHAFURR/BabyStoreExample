using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
    }

    public class ProductMetaData
    {
        [Required(ErrorMessage = "The product name can not be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please, enter a product name" +
            "betweem 3 and 50 characters  in length")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please, enter a product name" +
            "made up of letter and numbers only" )]
        [Display(Name = "Product Name")]
        public string Name;

        [Required(ErrorMessage = "The prodcut description can not be blank")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Please, enter a product" +
            "description between 10 and 200 characters in length")]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please, enter a product" +
            " description made up  of letter and numbers only")]
        [DataType(DataType.MultilineText)]
        public string Description;

        [Required(ErrorMessage = "The prica can not be balnk")]
        [Range(0.10, 10000.00, ErrorMessage = "Please, enter price between" +
            "0.10 and 10000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a " +
            "number up to two decimal places")]
        public decimal Price;
    }
}