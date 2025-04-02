namespace CoffeeShopAPI.Dtos
{
	public class OrderDto
	{
		public Guid OrderId { get; set; }
		public Guid UserID { get; set; }
		public string UserName { get; set; } 
		public DateTime OrderDate { get; set; }
		public string? PaymentMethod { get; set; }
		public decimal TotalAmount { get; set; }
		public int TableID { get; set; }
		public string TableName { get; set; }
		public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
	}
}
