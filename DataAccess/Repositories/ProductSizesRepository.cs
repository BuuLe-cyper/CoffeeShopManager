using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductSizesRepository : Repository<ProductSize>, IProductSizesRepository
    {
        public ProductSizesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> InactiveProductSize(int productSizeID)
        {
            try
            {
                ProductSize? existingProductSize = await _context.ProductSizes.FirstOrDefaultAsync(p => p.ProductSizeID == productSizeID && p.IsDeleted == false && p.IsActive == true);
                if (existingProductSize != null)
                {
                    existingProductSize.IsDeleted = true;
                    existingProductSize.IsActive = false;
                    existingProductSize.ModifyDate = DateTime.Now;

                    _context.ProductSizes.Update(existingProductSize);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
