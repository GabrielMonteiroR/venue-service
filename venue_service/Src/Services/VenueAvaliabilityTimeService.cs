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

        public VenueAvaliabilityTimeService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTime(CreateVenueAvaliabilityDto dto)
        {
            try
            {
                if (dto is null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid data", "The provided availability data is null.");

                var newAvailability = new VenueAvailabilityTime
                {
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    VenueId = dto.VenueId,
                    Price = dto.Price,
                };

                if (newAvailability.IsReserved)
                {
                    throw new HttpResponseException(
                        HttpStatusCode.BadRequest, "Venue already reserved", $"The venue with ID {newAvailability.VenueId} is already reserved for this time."
                    );
                }
                _context.VenueAvailabilities.Add(newAvailability);
                await _context.SaveChangesAsync();

                var responseDto = new VenueAvailabilityTimeResponseDto
                {
                    StartDate = newAvailability.StartDate,
                    EndDate = newAvailability.EndDate,
                    VenueId = newAvailability.VenueId,
                    Price = dto.Price
                };

                return responseDto;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
            }
        }

        public async Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto)
        {
            return await CreateVenueAvailabilityTime(dto);
        }

        public async Task<bool> DeleteVenueAvailabilityTimeAsync(int id)
        {
            var availability = await _context.VenueAvailabilities.FindAsync(id);
            if (availability == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not found", $"No availability found with ID {id}");
            }

            _context.VenueAvailabilities.Remove(availability);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<VenueAvaliabilityTimesResponseDto> ListAvaliableTimesByVenue(int venueId)
        {
            try
            {
                var avaliableTimes = await _context.VenueAvailabilities
                    .Where(v => v.VenueId == venueId)
                    .Select(v => new VenueAvailabilityTimeResponseDto
                    {
                        StartDate = v.StartDate,
                        EndDate = v.EndDate,
                        VenueId = v.VenueId,
                        Price = v.Price
                    })
                    .ToListAsync();

                return new VenueAvaliabilityTimesResponseDto
                {
                    Message = avaliableTimes.Any() ? $"Available times found." : $"No available times for venue with id {venueId}.",
                    venueAvailabilityTimeResponseDtos = avaliableTimes
                }; 
            } catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
            }
        }

    }
}
