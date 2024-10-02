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
    public class TableRepository : Repository<Table>, ITableRepository
    {
        public TableRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Table> CreateTableAsync(string description)
        {
            var table = new Table
            {
                Description = description,
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            //table.QRCodeTable = _generateQRCode.GenerateQRCodeForTable(table.TableID);
            //await _context.SaveChangesAsync();

            return table;
        }
        public async Task SoftDeleteTable(int id)
        {
            try
            {
                var tableExit = await _context.Tables.SingleOrDefaultAsync(t => t.TableID == id && t.IsDeleted == false && t.IsActive == true);
                if (tableExit != null)
                {
                    tableExit.IsActive = false;
                    tableExit.IsDeleted = true;
                    tableExit.ModifyDate = DateTime.Now;
                    _context.Tables.Update(tableExit);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateTableAsync(Table table)
        {
            var trackedEntity = _context.ChangeTracker.Entries<Table>()
                                     .FirstOrDefault(e => e.Entity.TableID == table.TableID);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
            }

            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

    }
}
