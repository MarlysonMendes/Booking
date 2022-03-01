using AutoMapper;
using CwkBooking.Api.Dtos;
using CwkBooking.Domain.Models;

namespace Cwk.Booking.Api.AutoMapper
{
    public class HotelMappingProfiles : Profile
    {
        public HotelMappingProfiles()
        {
            CreateMap<HotelCreateDto, Hotel>();
            CreateMap<Hotel, HotelGetDto>();
        }
    }
}
