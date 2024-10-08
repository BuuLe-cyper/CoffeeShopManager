using AutoMapper;
using BussinessObjects.DTOs.Message;
using BussinessObjects.DTOs.Tables;
using CoffeeShop.ViewModels.Message;
using CoffeeShop.ViewModels.Tables;
using BussinessObjects.DTOs;
using CoffeeShop.ViewModels;
using DataAccess.Models;

namespace CoffeeShop.AutoMapper
{
    public class MappingProfileView : Profile
    {
        public MappingProfileView()
        {
            CreateMap<MessageVM,MessageDTO>().ReverseMap();
            CreateMap<TableVM,TableDTO>().ReverseMap();
            CreateMap<UsersDTO, UserVM>().ReverseMap();
            CreateMap<SizeViewDto, SizeVM>().ReverseMap();
            CreateMap<CategoryViewDto, CategoryVM>().ReverseMap();
            CreateMap<ProductViewDto, ProductVM>().ReverseMap();
            CreateMap<ProductDto, ProductVM>().ReverseMap();
            CreateMap<CategoryDto, CategoryVM>().ReverseMap();
            CreateMap<SizeDto, SizeVM>().ReverseMap();
            CreateMap<ProductSizeDto, ProductSizeVM>().ReverseMap();
            CreateMap<ProductSizesViewDto, ProductSizeVM>().ReverseMap();
            CreateMap<MenuItemVMDto, ProductSizesViewDto>().ReverseMap();
            CreateMap<MenuItemVMDto, ProductSizeVM>().ReverseMap();
        }
    }
}
