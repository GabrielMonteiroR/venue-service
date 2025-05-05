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

        public async Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto)
        {
            try
            {
                var newAvailability = new VenueAvailabilityTime
                {
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    VenueId = dto.VenueId,
                    Price = dto.Price,
                    IsReserved = false,
                    TimeStatus = "NULL" 
                };

                _context.VenueAvailabilities.Add(newAvailability);
                await _context.SaveChangesAsync();

                var responseDto = new VenueAvailabilityTimeResponseDto
                {
                    StartDate = newAvailability.StartDate,
                    EndDate = newAvailability.EndDate,
                    VenueId = newAvailability.VenueId,
                    Price = newAvailability.Price,
                    Id = newAvailability.Id
                };

                return responseDto;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
            }
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
                        Id = v.Id,
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
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
            }
        }

        public async Task<VenueAvailabilityTime> UpdateAvaliabilityTime(int id, UpdateVenueAvaliabilityDto newTimeDto)
        {
            try
            {
                var OldAvaliableTime = await _context.VenueAvailabilities.FindAsync(id);
                if (OldAvaliableTime is null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not found", $"No availability found with ID {id}");
                };
                

                OldAvaliableTime.StartDate = newTimeDto.StartDate;
                OldAvaliableTime.EndDate = newTimeDto.EndDate;
                OldAvaliableTime.Price = newTimeDto.Price;
                OldAvaliableTime.IsReserved = newTimeDto.IsReserved;
                OldAvaliableTime.TimeStatus = newTimeDto.TimeStatus;

                return new VenueAvailabilityTime
                {
                    Id = OldAvaliableTime.Id,
                    StartDate = OldAvaliableTime.StartDate,
                    EndDate = OldAvaliableTime.EndDate,
                    VenueId = OldAvaliableTime.VenueId,
                    Price = OldAvaliableTime.Price,
                   TimeStatus = OldAvaliableTime.TimeStatus,
                   IsReserved = OldAvaliableTime.IsReserved
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    HttpStatusCode.InternalServerError,
                    "Unexpected error",
                    ex.InnerException?.Message ?? ex.Message
                );
            }

        }
    }
}
