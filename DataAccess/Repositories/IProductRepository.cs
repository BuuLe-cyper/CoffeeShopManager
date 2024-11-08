using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<bool> SoftDeleteProductEntity(int productID);
    }
}
