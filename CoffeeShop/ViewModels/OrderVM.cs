using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels
{
    public class OrderVM
    {
        [Display(Name = "Order Id")]
        public int OrderID { get; set; }
    }
}
