using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;
using venue_service.Src.Models;

namespace venue_service.Src.Services
{
    public class VenueService
    {
        private readonly DatabaseContext _context;

        public VenueService(DatabaseContext context)
        {
            context = _context;
        }

        public async Task<VenueResponseDto> CreateVenueAsync(CreateVenueDto dto)
        {
            try
            {
                var venue = new Venue
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Capacity = dto.Capacity,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Description = dto.Description,
                    AllowLocalPayment = dto.AllowLocalPayment,
                    VenueTypeId = dto.VenueTypeId,
                    Rules = dto.Rules,
                    OwnerId = dto.OwnerId,
                    VenueAvaliabilityId = dto.VenueAvaliabilityId
                };

                await _context.Venues.AddAsync(venue);
                return new VenueResponseDto
                {
                    Name = venue.Name,
                    Address = venue.Address,
                    Capacity = venue.Capacity,
                    Latitude = venue.Latitude,
                    Longitude = venue.Longitude,
                    Description = venue.Description,
                    AllowLocalPayment = venue.AllowLocalPayment,
                    VenueTypeId = venue.VenueTypeId,
                    Rules = venue.Rules,
                    OwnerId = venue.OwnerId,
                    VenueAvaliabilityId = venue.VenueAvaliabilityId
                };

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }

        }

    }
}