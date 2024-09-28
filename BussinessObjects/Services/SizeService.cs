using AutoMapper;
using BussinessObjects.DTOs;
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
            ArgumentNullException.ThrowIfNull(nameof(sizeDto.sizeName));
            var sizeNameExist = await GetSizeName(sizeDto.sizeName);
            if(sizeNameExist == null)
            {
            var entity = _mapper.Map<Size>(sizeDto);
            return await _sizeRepository.AddSizeEntity(entity);
        }
            else
            {
                throw new Exception("The Size Name Existed");
            }
         
        }

        public async Task<IEnumerable<Size>> GetAllSize()
        {
            var sizes = await _sizeRepository.GetAllAsync(s => s.IsDeleted == false && s.IsActive == true);
            return sizes;
        }

        public async Task<Size> GetSize(int sizeID)
        {
            var size = await _sizeRepository.GetAsync(s => s.IsDeleted == false && s.IsActive == true && s.SizeID == sizeID);
            return size;
        }

        public async Task<Size> GetSizeName(string sizeName)
        {
            var size = await _sizeRepository.GetAsync(s => s.IsDeleted == false && s.IsActive == true && s.SizeName == sizeName);
            return size;
        }

        public async Task<bool> SoftDeleteSize(int sizeID)
        {
            return await _sizeRepository.SoftDeleteSizeEntity(sizeID);
        }

        public async Task<bool> UpdateSize(Size size)
        {
            ArgumentNullException.ThrowIfNull(nameof(size));
            var sizeExists = await _sizeRepository.GetAsync(s => s.SizeID == size.SizeID && s.IsDeleted == false && s.IsActive == true) ?? throw new Exception("Size Not Found");
            if (sizeExists != null)
            {
                // ensuring no permission modify this field
                size.IsDeleted = false;
                size.IsActive = true;
                size.CreateDate = sizeExists.CreateDate;
                size.ModifyDate = sizeExists.ModifyDate;

                await _sizeRepository.UpdateAsync(size);
                return true;
            }
            return false;
        }
    }
}
