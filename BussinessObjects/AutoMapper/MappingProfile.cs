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
        }
    }
}
