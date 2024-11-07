using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ITableRepository : IRepository<Table>
    {
        Task<Table> CreateTableAsync(string description);
        Task UpdateTableAsync(Table table);
        Task SoftDeleteTable(int id);
    }
}
