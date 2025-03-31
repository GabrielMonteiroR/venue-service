using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;
using venue_service.Src.Models;

namespace venue_service.Src.Services
{
    public class VenueAvaliabilityTimeService : IVenueAvaliabilityTime
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public VenueAvaliabilityTimeService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTime(CreateVenueAvaliabilityDto dto)
        {
            try
            {
                if (dto is null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid data", "The provided availability data is null.");

                var newAvailability = _mapper.Map<VenueAvailabilityTime>(dto);

                if (newAvailability.IsReserved)
                {
                    throw new HttpResponseException(
                        HttpStatusCode.BadRequest, "Venue already reserved", $"The venue with ID {newAvailability.VenueId} is already reserved for this time."
                    );
                }
                _context.VenueAvailabilities.Add(newAvailability);
                await _context.SaveChangesAsync();
                return _mapper.Map<VenueAvailabilityTimeResponseDto>(dto);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
            }
        }







    }
}
