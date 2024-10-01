using AutoMapper;
using BussinessObjects.DTOs;
using CoffeeShop.ViewModels;
using DataAccess.Models;

namespace CoffeeShop.AutoMapper
{
    public class MappingProfileView : Profile
    {
        public MappingProfileView()
        {
            CreateMap<UsersDTO, UserVM>().ReverseMap();
            CreateMap<SizeViewDto, SizeVM>().ReverseMap();
            CreateMap<CategoryViewDto, CategoryVM>().ReverseMap();
            CreateMap<ProductViewDto, ProductVM>().ReverseMap();
            CreateMap<ProductDto, ProductVM>().ReverseMap();
        }
    }
}

