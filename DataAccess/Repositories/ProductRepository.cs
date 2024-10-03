using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<bool> SoftDeleteProductEntity(int productID)
        {
            try
            {
                Product? existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productID && p.IsDeleted == false && p.IsActive == true && p.IsAvailable == true);
                if (existingProduct != null)
                {
                    existingProduct.IsDeleted = true;
                    existingProduct.IsActive = false;
                    existingProduct.IsAvailable = false; 
                    existingProduct.ModifyDate = DateTime.Now;

                    _context.Products.Update(existingProduct);
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
