﻿namespace BussinessObjects.DTOs
{
    public class OrderDetailDTO
    {
		public int OrderDetailId { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public float Discount { get; set; }
		public Guid OrderID { get; set; }
		public int ProductSizeID { get; set; }

		public string ProductName { get; set; }
		public string SizeName { get; set; }
	}
}
