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

        public Task<VenueAvailabilityTimeResponseDto> AssignAvaliableTime(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto)
        {
            try
            {
                var newAvailability = new VenueAvailabilityTime
                {
                    VenueId = dto.VenueId,
                    StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
                    Price = dto.Price,
                    TimeStatus = "TimeStatusEnum.Disponivel",
                    IsReserved = false
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
                        Price = v.Price,
                        IsReserved = v.IsReserved,
                        TimeStatus = v.TimeStatus,
                        UserId = v.UserId
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
                var existing = await _context.VenueAvailabilities.FindAsync(id);
                if (existing is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not found", $"No availability found with ID {id}");

                existing.StartDate = DateTime.SpecifyKind(newTimeDto.StartDate, DateTimeKind.Utc);
                existing.EndDate = DateTime.SpecifyKind(newTimeDto.EndDate, DateTimeKind.Utc);
                existing.Price = newTimeDto.Price;

                await _context.SaveChangesAsync();

                return new VenueAvailabilityTime
                {
                    Id = existing.Id,
                    StartDate = existing.StartDate,
                    EndDate = existing.EndDate,
                    VenueId = existing.VenueId,
                    Price = existing.Price,
                    TimeStatus = existing.TimeStatus,
                    IsReserved = existing.IsReserved
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
