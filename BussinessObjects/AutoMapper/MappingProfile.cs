using AutoMapper;
using BussinessObjects.DTOs;
using DataAccess.Models;
namespace BussinessObjects.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsersDTO, User>().ReverseMap();
            CreateMap<SizeDto,Size>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<SizeViewDto, Size>().ReverseMap();
            CreateMap<CategoryViewDto, Category>().ReverseMap();
            CreateMap<ProductViewDto, Product>().ReverseMap();
        }
    }
}
