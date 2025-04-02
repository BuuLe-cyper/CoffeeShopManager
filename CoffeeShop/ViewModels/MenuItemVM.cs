namespace CoffeeShop.ViewModels
{
	public class MenuItemVM
	{
		public int ProductSizeID { get; set; }
		public int ProductID { get; set; }
		public ProductVM Product { get; set; }
		public int SizeID { get; set; }
		public SizeVM Size { get; set; }
		public decimal Price { get; set; }
		public decimal OriginalPrice { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? ModifyDate { get; set; }
	}
}
