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
        }
    }
}
