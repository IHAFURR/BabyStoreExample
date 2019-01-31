using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    public class Address
    {
        [Required]
        [Display(Name = "Address line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address line 2")]
        public string AddressLine2 { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PostCode { get; set; }
    }
}