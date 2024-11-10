using AutoMapper;
using DataAccess.Models;
using BussinessObjects.DTOs;
using BussinessObjects.DTOs.Message;
using BussinessObjects.DTOs.Tables;
using DataAccess.Models;
namespace BussinessObjects.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SizeDto, Size>().ReverseMap();
            CreateMap<TableDTO,Table>().ReverseMap();
            CreateMap<MessageDTO, Message>().ReverseMap();
            CreateMap<UsersDTO, User>().ReverseMap();
            CreateMap<SizeDto,Size>().ReverseMap();
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

        }
    }
}
