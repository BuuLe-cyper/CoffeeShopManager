using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Dtos
{
    public class ProductCreateDto
    {
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        [FromForm]
        public IFormFile File { get; set; } = default!;
        public string ImageUrl { get; set; }
    }
}
