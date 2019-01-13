using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BabyStore.Models
{
    public partial class ProductImage
    {
        public int ID { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }
        public virtual ICollection<ProductImageMapping> ProductImageMappings { get; set; }
    }
}