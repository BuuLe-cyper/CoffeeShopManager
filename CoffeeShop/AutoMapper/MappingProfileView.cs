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
            CreateMap<Size, SizeVM>()
             .ForMember(dest => dest.ModifyDate, opt => opt.MapFrom(src => src.ModifyDate));
        }
    }
}

