using AutoMapper;
using venue_service.Src.Dtos;
using venue_service.Src.Models;

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
