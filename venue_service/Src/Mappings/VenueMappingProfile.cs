using AutoMapper;
using venue_service.Src.Dtos;
using venue_service.Src.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace venue_service.Src.Mappings
{
    public class VenueMappingProfile : Profile
    {
        public VenueMappingProfile()
        {
            CreateMap<CreateVenueDto, Venue>();
            CreateMap<Venue, VenueResponseDto>();
        }
    }
}
