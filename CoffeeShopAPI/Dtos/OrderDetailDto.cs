namespace CoffeeShopAPI.Dtos
{
	public class OrderDetailDto
	{
		public int OrderDetailId { get; set; }
		public int ProductId { get; set; }
		public string? ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public float Discount { get; set; }
		public Guid OrderID { get; set; }
		public int ProductSizeID { get; set; }
		public string? ProductSizeName { get; set; }
	}
}
