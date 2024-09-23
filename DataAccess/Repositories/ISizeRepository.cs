using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ISizeRepository : IRepository<Size>
    {
        public Task<bool> AddSizeEntity(Size size);
        public Task<bool> SoftDeleteSizeEntity(int sizeID);
    }
}
