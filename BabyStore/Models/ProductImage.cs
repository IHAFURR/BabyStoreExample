using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    public partial class ProductImage
    {
        public int ID { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }
    }
}