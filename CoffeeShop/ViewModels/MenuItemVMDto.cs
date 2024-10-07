using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.ViewModels
{
    public class MenuItemVMDto
    {
        public int ProductSizeID { get; set; }
        public int ProductID { get; set; }
        public List<SizePriceVMDto> SizePrices { get; set; } = new List<SizePriceVMDto>();
        public decimal OriginalPrice { get; set; }
    }
    public class SizePriceVMDto
    {
        public int SizeID { get; set; }
        public decimal Price { get; set; }
    }

}