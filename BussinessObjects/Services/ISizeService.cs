using BussinessObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface ISizeService
    {
        public Task<bool> AddSize(SizeDto size);
        public Task<bool> SoftDeleteSize(int sizeID);
        public Task<IEnumerable<SizeDto>> GetAllSize();
        public Task<SizeDto> GetSize(int sizeID);
    }
}
