using DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels
{
    public class ProductVM
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string ProductDescription { get; set; }

        [Range(0, 1, ErrorMessage = "Discount must be between 0 and 1.")]
        public float Discount { get; set; }

        [Url(ErrorMessage = "Invalid image URL format.")]
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        [DisplayFormat(NullDisplayText = "Modify Not Yet")]
        [Display(Name = "Modify Date")]
        public DateTime? ModifyDate { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }

        public string FormattedPrice { get; set; }
    }

}
