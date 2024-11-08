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
            try
            {
                sizeDto.sizeName = sizeDto.sizeName.Trim().ToUpper();
                var sizeNameExist = await GetSizeName(sizeDto.sizeName);
                var deletedSizeName = await _sizeRepository.GetAsync(item => item.SizeName == sizeDto.sizeName
                                                                          && item.IsDeleted == true && item.IsActive == false);
                if (sizeNameExist == null && (deletedSizeName != null || deletedSizeName == null))
                {
                    var entity = _mapper.Map<Size>(sizeDto);
                    await _sizeRepository.CreateAsync(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task<IEnumerable<SizeViewDto>> GetAllSize()
        {
            var sizes = await _sizeRepository.GetAllAsync(s => s.IsDeleted == false && s.IsActive == true);
            return _mapper.Map<IEnumerable<SizeViewDto>>(sizes);
        }

        public async Task<SizeViewDto> GetSize(int sizeID)
        {
            var size = await _sizeRepository.GetAsync(s => s.IsDeleted == false && s.IsActive == true && s.SizeID == sizeID);
            return _mapper.Map<SizeViewDto>(size);
        }

        public async Task<SizeViewDto> GetSizeName(string sizeName)
        {
            var size = await _sizeRepository.GetAsync(s => s.IsDeleted == false && s.IsActive == true && s.SizeName == sizeName);
            return _mapper.Map<SizeViewDto>(size);
        }

        public async Task<bool> SoftDeleteSize(int sizeID)
        {
            return await _sizeRepository.SoftDeleteSizeEntity(sizeID);
        }

        public async Task<bool> UpdateSize(SizeViewDto size)
        {
            ArgumentNullException.ThrowIfNull(size, nameof(size));

            try
            {
                var sizeExists = await _sizeRepository.GetAsync(s => s.SizeID == size.SizeID && s.IsDeleted == false && s.IsActive == true)
                    ?? throw new Exception("Size Not Found");
                var sizeNameExist = await GetSizeName(size.SizeName);
                if (sizeNameExist == null)
                {
                    // Ensuring no permission to modify this field
                    size.IsDeleted = false;
                    size.IsActive = true;
                    size.CreateDate = sizeExists.CreateDate;
                    size.ModifyDate = DateTime.Now;

                    await _sizeRepository.UpdateAsync(_mapper.Map<Size>(size));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
