using BussinessObjects.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface ICategoryService
    {
        public Task<bool> AddCategory(CategoryDto categoryDto);
        public Task<bool> UpdateCategory(CategoryViewDto category);
        public Task<bool> SoftDeleteCategory(int categoryID);
        public Task<IEnumerable<CategoryViewDto>> GetAllCategory();
        public Task<CategoryViewDto> GetCategoryName(string categoryName);
        public Task<CategoryViewDto> GetCategory(int categoryID);
    }
}
