
using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    [MetadataType(typeof(CategoryMeteData))]
    public partial class Category
    { 
    }

    public class CategoryMeteData
    {
        [Required(ErrorMessage = "The Catalog name can not be blank")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "please enter  a category name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", 
            ErrorMessage = "Please, enter  a category name  beginning with a capital letter  and made" +
            " up  of letters  and space only")]
        [Display(Name = "Category Name")]
        public string Name;

    }
}