using BussinessObjects.DTOs.Tables;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface ITableService
    {
        Task<IEnumerable<TableDTO>> GetAllAsync(string? includeProperties = null);
        Task<TableDTO> CreateTableAsync(string description);
        Task<TableDTO> GetAsync(int id);
        Task UpdateTableAsync(TableDTO table);
        Task SoftDeleteTable(int id);
    }
}
