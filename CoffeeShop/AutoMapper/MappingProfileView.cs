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
            CreateMap<Size, SizeVM>().ForMember(dest => dest.ModifyDate, opt => opt.MapFrom(src => src.ModifyDate));
            CreateMap<UsersDTO, UserVM>().ReverseMap();
        }
    }
}
