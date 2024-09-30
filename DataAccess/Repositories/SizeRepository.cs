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
    public class SizeRepository : Repository<Size>, ISizeRepository 
    {
        public SizeRepository(ApplicationDbContext context) : base (context)
        {
        }

        public async Task<bool> SoftDeleteSizeEntity(int sizeID)
        {
            try
            {
                Size? existingSize = await _context.Sizes.SingleOrDefaultAsync(s => s.SizeID == sizeID && s.IsDeleted == false && s.IsActive == true);
                if (existingSize != null)
                {
                    existingSize.IsDeleted = true;
                    existingSize.IsActive = false;
                    existingSize.ModifyDate = DateTime.Now;

                    _context.Sizes.Update(existingSize);
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
