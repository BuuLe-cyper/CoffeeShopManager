using DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CoffeeShop.ViewModels
{
    public class ProductSizeVM
    {
        public int ProductSizeID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int SizeID { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
