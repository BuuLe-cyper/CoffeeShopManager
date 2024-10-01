using BussinessObjects.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface IProductService
    {
        public Task<bool> AddProduct(ProductDto productDto);
        public Task<bool> SoftDeleteProduct(int productID);
        public Task<bool> UpdateProduct(ProductDto product);
        public Task<IEnumerable<ProductViewDto>> GetAllProduct();
        public Task<ProductViewDto> GetProductName(string productName);
        public Task<IEnumerable<ProductViewDto>> GetProductsByCategoryID(int categoryID);
        public Task<ProductViewDto> GetProduct(int productID);
    }
}
