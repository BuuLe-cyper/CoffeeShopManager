using BussinessObjects.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface ISizeService
    {
        public Task<bool> AddSize(SizeDto sizeDto);
        public Task<bool> SoftDeleteSize(int sizeID);
        public Task<IEnumerable<SizeViewDto>> GetAllSize();
        public Task<SizeViewDto> GetSizeName(string sizeName);
        public Task<SizeViewDto> GetSize(int sizeID);
        public Task<bool> UpdateSize(SizeViewDto size);
    }
}
