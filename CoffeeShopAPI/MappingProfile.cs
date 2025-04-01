using AutoMapper;
using BussinessObjects.DTOs.Message;
using BussinessObjects.DTOs.Tables;
using BussinessObjects.DTOs;
using DataAccess.Models;
using CoffeeShopAPI.Dtos;

namespace CoffeeShopAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SizeDto, Size>().ReverseMap();
            CreateMap<TableDTO, Table>().ReverseMap();
            CreateMap<MessageDTO, Message>().ReverseMap();
            CreateMap<UsersDTO, User>().ReverseMap();
            CreateMap<SizeDto, Size>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<SizeViewDto, Size>().ReverseMap();
            CreateMap<CategoryViewDto, Category>().ReverseMap();
            CreateMap<ProductViewDto, Product>().ReverseMap();
            CreateMap<ProductSizesViewDto, ProductSize>().ReverseMap();

            // For Order
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
            CreateMap<ProductSizeDto, ProductSize>().ReverseMap();
            CreateMap<ProductCreateDto, ProductDto>().ReverseMap();
        }
    }
}
