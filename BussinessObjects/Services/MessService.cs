using AutoMapper;
using BussinessObjects.DTOs.Message;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public class MessService : IMessService
    {
        private readonly IMessRepository _messRepository;
        private IMapper _mapper;
        public MessService(IMessRepository messRepository, IMapper mapper) 
        { 
            _messRepository = messRepository;
            _mapper = mapper;
        }
        public async Task CreateMessage(MessageDTO messageDTO)
        {
            await _messRepository.CreateAsync( _mapper.Map<Message>(messageDTO));
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageByTableId(int tableId)
        {
            return _mapper.Map<IEnumerable<MessageDTO>>(await _messRepository.GetAllAsync(m => m.TableID.Equals(tableId) && m.IsActive == true && m.IsDeleted == false, includeProperties: "User"));
        }
    }
}
