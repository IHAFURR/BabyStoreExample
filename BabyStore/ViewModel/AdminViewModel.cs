using System.ComponentModel.DataAnnotations;
namespace BabyStore.ViewModel
{
    public class AdminViewModel
    {

    }

    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}