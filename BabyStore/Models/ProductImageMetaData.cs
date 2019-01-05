using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    public partial class ProductImageMetaData
    {
        [Display(Name = "File")]        
        public string FileName;
    }
}