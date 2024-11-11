using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels
{
    public class OrderVM
    {
        [Display(Name = "Order Id")]
        public Guid OrderId { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public int TableID { get; set; }
        public Table Table { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
