using AutoMapper;
using CwkBooking.Api.Dtos;
using CwkBooking.Domain.Models;

namespace CwkBooking.Api.AutoMapper
{
    public class RoomMappingProfiles : Profile
    {
        public RoomMappingProfiles()
        {
            CreateMap<Room, RoomGetDto>();
            CreateMap<RoomPostPutDto, Room>();
        }
    }
}
