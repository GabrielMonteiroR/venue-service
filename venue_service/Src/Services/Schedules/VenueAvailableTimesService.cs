using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.AvailabilityTimes;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.AvailableTimesInterfaces;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Services.Schedules;

public class VenueAvailableTimesService : IAvailableTimesService
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
                StartDate = DateTime.SpecifyKind(requestDto.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(requestDto.EndDate, DateTimeKind.Utc),
                VenueId = requestDto.VenueId,
                Price = requestDto.Price,
                IsReserved = requestDto.IsReserved,
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

            await _venueContext.VenueAvailabilities.AddAsync(request);
            await _venueContext.SaveChangesAsync();

            return new VenueAvailabilityTimeDto
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                VenueId = request.VenueId,
                Price = request.Price,
                IsReserved = request.IsReserved
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while creating the venue availability time.", ex.Message);
        }
    }

    public async Task<VenueAvailabilityTimeResponseDto> GetVenueAvailabilityTimes()
    {
        try
        {
            var availabilityTimes = await _venueContext.VenueAvailabilities
                .Include(v => v.Venue)
                .OrderBy(v => v.StartDate)
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
                }).ToList()
            };

            return response;

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving venue availability times.", ex.Message);
        }
    }

    public async Task<VenueAvailabilityTimeDto> GetVenueAvailabilityTimeById(int id)
    {
        try
        {
            var availabilityTime = await _venueContext.VenueAvailabilities.FindAsync(id);

            if (availabilityTime == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Venue availability time not found.", $"No venue availability time found with ID {id}.");
            }

            return new VenueAvailabilityTimeDto
            {
                Id = availabilityTime.Id,
                StartDate = availabilityTime.StartDate,
                EndDate = availabilityTime.EndDate,
                VenueId = availabilityTime.VenueId,
                Price = availabilityTime.Price,
                IsReserved = availabilityTime.IsReserved,
            };

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving the venue availability time.", ex.Message);
        }
    }

    public async Task<VenueAvailabilityTimeDto> UpdateVenueAvailabilityTime(int availabilityTimeId, UpdateVenueAvailabilityTimeDto requestDto)
    {
        try
        {
            var availabilityTimeToBeUpdated = await _venueContext.VenueAvailabilities.FindAsync(availabilityTimeId);

            if (availabilityTimeToBeUpdated == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Venue availability time not found.", $"No venue availability time found with ID {availabilityTimeId}.");
            }

            var start = requestDto.StartDate.HasValue
                ? DateTime.SpecifyKind(requestDto.StartDate.Value, DateTimeKind.Utc)
                : (DateTime?)null;

            var end = requestDto.EndDate.HasValue
                ? DateTime.SpecifyKind(requestDto.EndDate.Value, DateTimeKind.Utc)
                : (DateTime?)null;

            if (start.HasValue && end.HasValue && start >= end)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Start date must be earlier than end date.", $"{start} is after {end}");
            }

            if (start.HasValue && end.HasValue)
            {
                var hasOverlap = await _venueContext.VenueAvailabilities
                    .AnyAsync(x =>
                        x.VenueId == availabilityTimeToBeUpdated.VenueId &&
                        x.Id != availabilityTimeId &&
                        (
                            start < x.EndDate && end > x.StartDate
                        ));

                if (hasOverlap)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict, "Venue availability time already exists.", $"The venue availability time for venue ID {availabilityTimeToBeUpdated.VenueId} from {start} to {end} already exists.");
                }
            }

            if (start.HasValue)
                availabilityTimeToBeUpdated.StartDate = start.Value;

            if (end.HasValue)
                availabilityTimeToBeUpdated.EndDate = end.Value;

            if (requestDto.Price.HasValue)
                availabilityTimeToBeUpdated.Price = requestDto.Price.Value;

            _venueContext.VenueAvailabilities.Update(availabilityTimeToBeUpdated);
            await _venueContext.SaveChangesAsync();

            return new VenueAvailabilityTimeDto
            {
                Id = availabilityTimeToBeUpdated.Id,
                StartDate = availabilityTimeToBeUpdated.StartDate,
                EndDate = availabilityTimeToBeUpdated.EndDate,
                VenueId = availabilityTimeToBeUpdated.VenueId,
                Price = availabilityTimeToBeUpdated.Price,
                IsReserved = availabilityTimeToBeUpdated.IsReserved
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while updating the venue availability time.", ex.Message);
        }
    }

    public async Task<DeleteVenueAvailabilityTimeDto> DeleteVenueAvailabilityTime(int id)
    {
        try
        {
            var availabilityTimeToBeDeleted = await _venueContext.VenueAvailabilities.FindAsync(id);
            if (availabilityTimeToBeDeleted == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Venue availability time not found.", $"No venue availability time found with ID {id}.");
            }

            if (availabilityTimeToBeDeleted.IsReserved)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Cannot delete reserved venue availability time.", $"The venue availability time with ID {id} is reserved and cannot be deleted.");
            }

            var availableTimeDeleted = availabilityTimeToBeDeleted;

            _venueContext.VenueAvailabilities.Remove(availabilityTimeToBeDeleted);
            await _venueContext.SaveChangesAsync();

            return new DeleteVenueAvailabilityTimeDto
            {
                Message = "Venue availability time deleted successfully.",
                Data = new VenueAvailabilityTimeDto
                {
                    Id = availableTimeDeleted.Id,
                    StartDate = availableTimeDeleted.StartDate,
                    EndDate = availableTimeDeleted.EndDate,
                    VenueId = availableTimeDeleted.VenueId,
                    Price = availableTimeDeleted.Price,
                    IsReserved = availableTimeDeleted.IsReserved,
                }
            };

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while deleting the venue availability time.", ex.Message);
        }
    }

    public async Task<VenueAvailabilityTimeResponseDto> GetAvailabilityTimesByVenueId(int venueId, bool? isReserved = null)
    {
        try
        {
            var query = _venueContext.VenueAvailabilities
            .Include(v => v.Venue)
            .Where(v => v.VenueId == venueId)
            .Where(v => v.EndDate >= DateTime.UtcNow)
            .OrderBy(v => v.StartDate)
            .AsQueryable();


            if (isReserved.HasValue)
            {
                query = query.Where(v => v.IsReserved == isReserved.Value);
            }

            var result = await query.ToListAsync();

            if (result == null || !result.Any())
            {
                return new VenueAvailabilityTimeResponseDto
                {
                    Message = "No venue availability times found for the specified venue ID.",
                    Data = new List<VenueAvailabilityTimeDto>()
                };
            }

            return new VenueAvailabilityTimeResponseDto
            {
                Message = "Available times retrieved successfully.",
                Data = result.Select(v => new VenueAvailabilityTimeDto
                {
                    Id = v.Id,
                    StartDate = v.StartDate,
                    EndDate = v.EndDate,
                    VenueId = v.VenueId,
                    Price = v.Price,
                    IsReserved = v.IsReserved,
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving venue availability times by venue ID.", ex.Message);
        }
    }

    public async Task<bool> SetTrueToIsReserved(int AvailabilityTimeId)
    {
        try
        {
            var entity = await _venueContext.VenueAvailabilities.FindAsync(AvailabilityTimeId);

            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "Venue availability time not found.", $"No venue availability time found with ID {AvailabilityTimeId}.");
            }

            entity.IsReserved = true;

            await _venueContext.SaveChangesAsync();

            return true;

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while setting IsReserved to true.", ex.Message);
        }
    }
}