using BussinessObjects.DTOs.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface IMessService
    {
        Task<IEnumerable<MessageDTO>> GetMessageByTableId(int tableId);
        Task CreateMessage(MessageDTO messageDTO);
    }
}
