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
    public class MessRepository : Repository<Message>, IMessRepository
    {
        public MessRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task UpdateMessagesByTableIdAsync(int tableId)
        {
            var messages = await _context.Messages
                                         .Where(m => m.TableID == tableId && m.IsActive)
                                         .ToListAsync();

            if (!messages.Any()) return;

            var currentTime = DateTime.Now;

            foreach (var message in messages)
            {
                var trackedEntity = _context.ChangeTracker.Entries<Message>()
                                             .FirstOrDefault(e => e.Entity.MessageID == message.MessageID);

                if (trackedEntity != null)
                {
                    _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
                }
                _context.Messages.Attach(message);

                message.IsActive = false;
                message.IsDeleted = true;
                message.ModifyDate = currentTime;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessAsyncByHour()
        {
            var currentTime = DateTime.Now;

            var messagesToDelete = await _context.Messages
                                                 .AsNoTracking()
                                                 .Where(m => m.IsActive && !m.IsDeleted && m.CreateDate < currentTime.AddMinutes(-40))
                                                 .ToListAsync();

            if (!messagesToDelete.Any()) return;

            foreach (var message in messagesToDelete)
            {
                var trackedEntity = _context.ChangeTracker.Entries<Message>()
                                             .FirstOrDefault(e => e.Entity.MessageID == message.MessageID);

                if (trackedEntity != null)
                {
                    _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
                }

                _context.Messages.Attach(message);

                message.IsActive = false;
                message.IsDeleted = true;
                message.ModifyDate = currentTime;
            }
            await _context.SaveChangesAsync();
        }
    }
}
