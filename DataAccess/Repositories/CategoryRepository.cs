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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> SoftDeleteCategoryEntity(int categoryID)
        {
            try
            {
                Category? existingCate = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryID == categoryID && c.IsDeleted == false && c.IsActive == true);
                if (existingCate != null)
                {
                    existingCate.IsDeleted = true;
                    existingCate.IsActive = false;
                    existingCate.ModifyDate = DateTime.Now;

                    _context.Categories.Update(existingCate);
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
