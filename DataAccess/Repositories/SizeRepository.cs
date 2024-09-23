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

        public SizeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> AddSizeEntity(Size size)
        {
            try
            {
                Size sizeDto = new Size
                {
                    SizeID = size.SizeID,
                    SizeName = size.SizeName,
                    IsDeleted = false,
                    IsActive = true,
                    CreateDate = DateTime.Now,
                    ModifyDate = null
                };
                await _context.Sizes.AddAsync(sizeDto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<bool> SoftDeleteSizeEntity(int sizeID)
        {
            try
            {
                Size? existingSize = await _context.Sizes.SingleOrDefaultAsync(s => s.SizeID == sizeID);
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
