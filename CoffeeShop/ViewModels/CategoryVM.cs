using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels
{
    public class CategoryVM
    {
        [Display(Name = "Category Id")]
        public int CategoryID { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(NullDisplayText = "Modify Not Yet")]
        [Display(Name = "Modify Date")]
        public DateTime? ModifyDate { get; set; }
    }
}
