using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Enums;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IMapper _mapper;
        public SizeService(ISizeRepository sizeRepository, IMapper mapper)
        {
            _sizeRepository = sizeRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddSize(SizeDto sizeDto)
        {
            ArgumentNullException.ThrowIfNull(nameof(sizeDto));

            if (!Enum.IsDefined(typeof(SizeType), sizeDto.SizeID))
            {
                throw new ArgumentException("Invalid size ID.");
            }
            var sizeEntity = _mapper.Map<Size>(sizeDto);
            return await _sizeRepository.AddSizeEntity(sizeEntity);
        }

        public async Task<IEnumerable<SizeDto>> GetAllSize()
        {
            var listSize = await _sizeRepository.GetAllAsync(item => item.IsDeleted == false && item.IsActive == true);
            return _mapper.Map<IEnumerable<SizeDto>>(listSize);
        }

        public async Task<SizeDto> GetSize(int sizeID)
        {
            var size = await _sizeRepository.GetAsync(item => item.SizeID == sizeID && item.IsDeleted == false && item.IsActive == true);
            return _mapper.Map<SizeDto>(size);
        }

        public async Task<bool> SoftDeleteSize(int sizeID)
        {
            return await _sizeRepository.SoftDeleteSizeEntity(sizeID);
        }
    }
}
