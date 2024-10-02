using AutoMapper;
using BussinessObjects.DTOs.Tables;
using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repositories;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;
        public TableService(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TableDTO>> GetAllAsync(string? includeProperties = null)
        {
            return _mapper.Map<IEnumerable<TableDTO>>(await _tableRepository.GetAllAsync(t=> t.IsActive == true && t.IsDeleted == false, includeProperties: includeProperties));
        }
        public async Task<TableDTO> CreateTableAsync(string description)
        {
           return _mapper.Map<TableDTO>(await _tableRepository.CreateTableAsync(description));
        }

        public async Task<TableDTO> GetAsync(int id)
        {
           return _mapper.Map<TableDTO>(await _tableRepository.GetAsync(t=> t.TableID == id && t.IsDeleted == false && t.IsActive == true));
        }

        public async Task UpdateTableAsync(TableDTO table)
        {
            await _tableRepository.UpdateTableAsync(_mapper.Map<Table>(table));
        }

        public async Task SoftDeleteTable(int id)
        {
            await _tableRepository.SoftDeleteTable(id);
        }
    }
}
