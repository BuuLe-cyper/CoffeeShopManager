using AutoMapper;
using BussinessObjects.DTOs;
using CoffeeShop.ViewModels;

public class MappingProfileView : Profile
{
    public MappingProfileView()
    {
        CreateMap<UsersDTO, UserVM>().ReverseMap();
    }
}
