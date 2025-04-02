using DataAccess.Models;

namespace CoffeeShop.ViewModels
{
    public class OrderDetailVM
    {
        public int OrderDetailId { get; set; }
        public int ProductSizeID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public Guid OrderID { get; set; }

		public string ProductName { get; set; }
		public string SizeName { get; set; }
	}

    public class CartData
    {
        public List<OrderDetailVM> CartItems { get; set; }
        public Guid? UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public int TableId { get; set; }
        public string paymentMethod { get; set; }

    }
}
