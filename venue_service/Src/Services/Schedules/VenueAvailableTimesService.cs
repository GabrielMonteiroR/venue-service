using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.AvailabilityTimes;
using venue_service.Src.Exceptions;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Services.Schedules;

public class VenueAvailableTimesService
{
    private readonly VenueContext _venueContext;

    public VenueAvailableTimesService(VenueContext venueContext)
    {
        _venueContext = venueContext;
    }

    public async Task<VenueAvailabilityTimeDto> CreateVenueAvailabilityTime(CreateVenueAvailabilityTimeDto requestDto)
    {
        try
        {
            var request = new VenueAvailabilityTimeEntity
            {
                StartDate = requestDto.StartDate,
                EndDate = requestDto.EndDate,
                VenueId = requestDto.VenueId,
                Price = requestDto.Price,
                IsReserved = requestDto.IsReserved,
                UserId = requestDto.UserId
            };

            var hasOverlap = await _venueContext.VenueAvailabilities
 .AnyAsync(x =>
     x.VenueId == request.VenueId &&
     (
         (request.StartDate < x.EndDate && request.EndDate > x.StartDate)
     ));

            if (hasOverlap)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict, "Venue availability time already exists.", $"The venue availability time for venue ID {request.VenueId} from {request.StartDate} to {request.EndDate} already exists.");
            }

            if (request.StartDate >= request.EndDate)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, " Start date must be earlier than end date.", $"{request.StartDate} is bigger than {request.EndDate}");
            }


            if (request is null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid request data.", "Request data cannot be null or empty.");
            }

            await _venueContext.VenueAvailabilities.AddAsync(request);

            return new VenueAvailabilityTimeDto
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                VenueId = request.VenueId,
                Price = request.Price,
                IsReserved = request.IsReserved,
                UserId = request.UserId
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while creating the venue availability time.", ex.Message);
        }
    }
}

public async Task<VenueAvailabilityTimeResponseDto> GetVenueAvailabilityTimeResponses()
    {
        try
        {
            var availabilityTimes = await _venueContext.VenueAvailabilities
                .Include(v => v.Venue)
                .ToListAsync();

            if (availabilityTimes == null || !availabilityTimes.Any())
            {
                return new VenueAvailabilityTimeResponseDto
                {
                    Message = "No venue availability times found.",
                    Data = new List<VenueAvailabilityTimeDto>()
                };
            }

            var response = new VenueAvailabilityTimeResponseDto
            {
                Message = "Venue availability times retrieved successfully.",
                Data = availabilityTimes.Select(v => new VenueAvailabilityTimeDto
                {
                    Id = v.Id,
                    StartDate = v.StartDate,
                    EndDate = v.EndDate,
                    VenueId = v.VenueId,
                    Price = v.Price,
                    IsReserved = v.IsReserved,
                    UserId = v.UserId
                }).ToList()
            };

            return response;

        } catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving venue availability times.", ex.Message);
        }
    }


}