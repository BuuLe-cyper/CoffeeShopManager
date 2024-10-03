using BussinessObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface IProductSizesService
    {
        public Task<bool> AddProductSize(ProductSizeDto productSizeDto);
        //public Task<bool> SoftDeleteProductSize(int productSizeID);
        public Task<bool> UpdateProductSize(ProductSizeDto productSizeDto);
        public Task<IEnumerable<ProductSizesViewDto>> GetAllProductSizes();
        public Task<IEnumerable<ProductSizesViewDto>> GetProductName(string productName);
        public Task<IEnumerable<ProductSizesViewDto>> GetProductSizeByCategoryID(int categoryID);
        public Task<ProductSizesViewDto> GetProductSize(int productID);
    }
}
