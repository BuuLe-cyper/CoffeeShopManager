using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels
{
    public class SizeVM
    {
        [Display(Name = "Size Id")]
        public int SizeID { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Size Name")]
        public string SizeName { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(NullDisplayText = "Modify Not Yet")]
        [Display(Name = "Modify Date")]
        public DateTime? ModifyDate { get; set; }
    }
}
