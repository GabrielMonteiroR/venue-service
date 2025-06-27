using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.ImageStorageInterfaces;
using venue_service.Src.Interfaces.VenueInterfaces;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Services.Venue
{
    public class VenueService : IVenueService
    {
        private readonly VenueContext _venueContext;
        private readonly UserContext _userContext;
        private readonly IStorageService _storageService;

        public VenueService(VenueContext context, IStorageService storageService, UserContext userContext)
        {
            _venueContext = context;
            _storageService = storageService;
            _userContext = userContext;
        }

        public async Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto)
        {
            try
            {
                var venue = new VenueEntity
                {
                    Name = dto.Name,
                    Street = dto.Street,
                    Number = dto.Number,
                    Complement = dto.Complement,
                    Neighborhood = dto.Neighborhood,
                    City = dto.City,
                    State = dto.State,
                    PostalCode = dto.PostalCode,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Description = dto.Description,
                    Capacity = dto.Capacity,
                    Rules = dto.Rules,
                    VenueTypeId = dto.VenueTypeId,
                    OwnerId = dto.OwnerId
                };

                venue.VenueImages = dto.ImageUrls.Select(url => new VenueImageEntity
                {
                    ImageUrl = url,
                    FileName = Path.GetFileName(new Uri(url).LocalPath),
                    Venue = venue
                }).ToList();

                _venueContext.Venues.Add(venue);
                await _venueContext.SaveChangesAsync();

                return new VenueResponseDto
                {
                    Id = venue.Id,
                    Name = venue.Name,
                    Street = venue.Street,
                    Number = venue.Number,
                    Complement = venue.Complement,
                    Neighborhood = venue.Neighborhood,
                    City = venue.City,
                    State = venue.State,
                    PostalCode = venue.PostalCode,
                    Latitude = venue.Latitude,
                    Longitude = venue.Longitude,
                    Description = venue.Description,
                    Capacity = venue.Capacity,
                    Rules = venue.Rules,
                    VenueTypeId = venue.VenueTypeId,
                    OwnerId = venue.OwnerId,
                    ImageUrls = venue.VenueImages.Select(i => i.ImageUrl).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    HttpStatusCode.InternalServerError,
                    "Error while registering the venue.",
                    $"{ex.Message}",
                    DateTime.UtcNow.ToString());
            }
        }

        public async Task<VenuesResponseDto> GetVenuesAsync(int? venueTypeId = null, DateTime? from = null, DateTime? to = null, int? minCapacity = null, int? maxCapacity = null, string? name = null, List<int>? sportId = null, bool? isReserved = false)
        {
            try
            {
                var query = _venueContext.Venues
                    .Include(v => v.VenueImages)
                    .Include(o => o.Owner)
                    .Include(vt => vt.VenueType)
                    .Include(s => s.VenueSports).ThenInclude(vs => vs.Sport)
                    .Include(va => va.VenueAvailabilityTimes)
                    .Where(vat => vat.VenueAvailabilityTimes.Any(vat => vat.EndDate >= DateTime.UtcNow))
                    .AsQueryable();

                if (venueTypeId.HasValue)
                    query = query.Where(v => v.VenueTypeId == venueTypeId.Value);

                if (isReserved.HasValue)
                {
                    query = query.Where(v => v.VenueAvailabilityTimes.Any(va => va.IsReserved == isReserved.Value));
                }

                if (sportId != null && sportId.Count > 0)
                {
                    query = query.Where(v => v.VenueSports.Any(vs => sportId.Contains(vs.SportId)));
                }

                if (from.HasValue || to.HasValue)
                {
                    query = query.Where(v =>
                        v.VenueAvailabilityTimes.Any(va =>
                            (!from.HasValue || va.StartDate >= from.Value) &&
                            (!to.HasValue || va.EndDate <= to.Value)
                        )
                    );
                }

                if (minCapacity.HasValue)
                    query = query.Where(v => v.Capacity >= minCapacity.Value);

                if (maxCapacity.HasValue)
                    query = query.Where(v => v.Capacity <= maxCapacity.Value);

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(v => EF.Functions.Like(v.Name, $"%{name}%"));

                var venues = await query.ToListAsync();

                if (venues.Count == 0 || venues is null)
                {
                    return new VenuesResponseDto
                    {
                        Message = "No venues found with the specified filters",
                        Data = new List<VenueResponseDto>()
                    };
                }

                var venueDtos = venues.Select(v => new VenueResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Street = v.Street,
                    Number = v.Number,
                    Complement = v.Complement,
                    Neighborhood = v.Neighborhood,
                    City = v.City,
                    State = v.State,
                    PostalCode = v.PostalCode,
                    Capacity = v.Capacity,
                    Latitude = v.Latitude,
                    Longitude = v.Longitude,
                    Description = v.Description,
                    venueAvaliabilityTimes = v.VenueAvailabilityTimes.Select(t => new VenueAvailabilityTimeResponseDto
                    {
                        Id = t.Id,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        Price = t.Price,
                        VenueId = t.VenueId,
                        IsReserved = t.IsReserved,
                    }).ToList(),
                    Sports = v.VenueSports.Select(s => s.Sport.Name).ToList(),
                    VenueTypeId = v.VenueTypeId,
                    venueTypeName = v.VenueType.Name,
                    Rules = v.Rules,
                    OwnerId = v.OwnerId,
                    OwnerName = v.Owner.FirstName,
                    ImageUrls = v.VenueImages?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
                }).ToList();

                return new VenuesResponseDto
                {
                    Message = "Venues retrieved successfully with applied filters",
                    Data = venueDtos
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task<VenuesResponseDto> DeleteVenuesAsync(int[] ids)
        {
            try
            {
                var venues = await _venueContext.Venues
                    .Include(v => v.VenueImages)
                    .Where(v => ids.Contains(v.Id)).ToListAsync();

                if (venues.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");
                }

                foreach (var venue in venues)
                {
                    foreach (var image in venue.VenueImages)
                    {
                        var parsed = _storageService.ParseSupabaseUrl(image.ImageUrl);
                        if (parsed != null)
                            await _storageService.DeleteFileAsync(parsed.Value.Bucket, parsed.Value.Path);
                    }
                }

                _venueContext.VenueImages.RemoveRange(venues.SelectMany(v => v.VenueImages));
                _venueContext.Venues.RemoveRange(venues);
                await _venueContext.SaveChangesAsync();

                return new VenuesResponseDto
                {
                    Message = "Venues deleted successfully",
                    Data = venues.Select(v => new VenueResponseDto
                    {
                        Id = v.Id,
                        Name = v.Name
                    })
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task<VenueResponseDto> UpdateVenueAsync(int venueId, UpdateVenueRequestDto dto)
        {
            try
            {
                var venue = await _venueContext.Venues.FindAsync(venueId);

                if (venue is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found", "The specified venue could not be located.");

                venue.Name = dto.Name ?? venue.Name;
                venue.Street = dto.Street ?? venue.Street;
                venue.Number = dto.Number ?? venue.Number;
                venue.Complement = dto.Complement ?? venue.Complement;
                venue.Neighborhood = dto.Neighborhood ?? venue.Neighborhood;
                venue.City = dto.City ?? venue.City;
                venue.State = dto.State ?? venue.State;
                venue.PostalCode = dto.PostalCode ?? venue.PostalCode;
                venue.Latitude = dto.Latitude ?? venue.Latitude;
                venue.Longitude = dto.Longitude ?? venue.Longitude;
                venue.Description = dto.Description ?? venue.Description;
                venue.Capacity = dto.Capacity ?? venue.Capacity;
                venue.Rules = dto.Rules ?? venue.Rules;
                venue.VenueTypeId = dto.VenueTypeId ?? venue.VenueTypeId;

                await _venueContext.SaveChangesAsync();

                var imageUrls = await _venueContext.VenueImages
                    .Where(img => img.VenueId == venue.Id)
                    .Select(img => img.ImageUrl)
                    .ToListAsync();

                return new VenueResponseDto
                {
                    Id = venue.Id,
                    Name = venue.Name,
                    Street = venue.Street,
                    Number = venue.Number,
                    Complement = venue.Complement,
                    Neighborhood = venue.Neighborhood,
                    City = venue.City,
                    State = venue.State,
                    PostalCode = venue.PostalCode,

                    Capacity = venue.Capacity,
                    Latitude = venue.Latitude,
                    Longitude = venue.Longitude,
                    Description = venue.Description,
                    VenueTypeId = venue.VenueTypeId,
                    Rules = venue.Rules,
                    OwnerId = venue.OwnerId,
                    ImageUrls = imageUrls
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    ex.Message);
            }
        }

        public async Task<VenuesResponseDto> ListVenuesByOwner(int id)
        {
            try
            {
                var owner = await _userContext.Users.FirstOrDefaultAsync(o => o.Id == id);
                if (owner is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Owner not found");

                var venues = await _venueContext.Venues
                    .Include(v => v.VenueImages)
                    .Include(o => o.Owner)
                    .Include(vt => vt.VenueType)
                    .Include(s => s.VenueSports).ThenInclude(vs => vs.Sport)
                    .Include(va => va.VenueAvailabilityTimes)
                    .Where(v => v.OwnerId == id)
                    .ToListAsync();

                if (venues.Count == 0)
                {
                    return new VenuesResponseDto
                    {
                        Message = "No venues found for this owner",
                        Data = new List<VenueResponseDto>()
                    };
                }

                var venueDtos = venues.Select(v => new VenueResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Street = v.Street,
                    Number = v.Number,
                    Complement = v.Complement,
                    Neighborhood = v.Neighborhood,
                    City = v.City,
                    State = v.State,
                    PostalCode = v.PostalCode,

                    Capacity = v.Capacity,
                    Latitude = v.Latitude,
                    Longitude = v.Longitude,
                    Description = v.Description,
                    VenueTypeId = v.VenueTypeId,
                    venueTypeName = v.VenueType?.Name,
                    Rules = v.Rules,
                    OwnerId = v.OwnerId,
                    OwnerName = v.Owner?.FirstName,
                    Sports = v.VenueSports.Select(s => s.Sport.Name).ToList(),
                    venueAvaliabilityTimes = v.VenueAvailabilityTimes.Select(t => new VenueAvailabilityTimeResponseDto
                    {
                        Id = t.Id,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        Price = t.Price,
                        VenueId = t.VenueId,
                        IsReserved = t.IsReserved,
                    }).ToList(),
                    ImageUrls = v.VenueImages?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
                }).ToList();

                return new VenuesResponseDto
                {
                    Message = "Venues retrieved successfully",
                    Data = venueDtos
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task<UpdateVenueImageResponseDto> AddVenueImagesAsync(UpdateVenueImageDto dto)
        {
            try
            {
                var venue = await _venueContext.Venues
                    .Include(v => v.VenueImages)
                    .FirstOrDefaultAsync(v => v.Id == dto.VenueId);

                if (venue == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");

                var newImages = dto.ImageUrls.Select(url => new VenueImageEntity
                {
                    VenueId = dto.VenueId,
                    ImageUrl = url,
                    FileName = Path.GetFileName(new Uri(url).LocalPath)
                }).ToList();

                foreach (var image in newImages)
                {
                    venue.VenueImages.Add(image);
                }

                await _venueContext.SaveChangesAsync();

                return new UpdateVenueImageResponseDto
                {
                    VenueId = dto.VenueId,
                    NewImageUrls = newImages.Select(i => i.ImageUrl).ToList(),
                    Message = "Images added successfully."
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task DeleteVenueImageAsync(int venueId, string imageUrl)
        {
            var venue = await _venueContext.Venues
                .Include(v => v.VenueImages)
                .FirstOrDefaultAsync(v => v.Id == venueId);

            if (venue == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");

            var image = venue.VenueImages.FirstOrDefault(i => i.ImageUrl == imageUrl);
            if (image == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Image not found");

            var parsed = _storageService.ParseSupabaseUrl(image.ImageUrl);
            if (parsed != null)
                await _storageService.DeleteFileAsync(parsed.Value.Bucket, parsed.Value.Path);

            _venueContext.VenueImages.Remove(image);
            await _venueContext.SaveChangesAsync();
        }

        public async Task<VenueResponseDto> GetVenueByIdAsync(int id)
        {
            try
            {
                var venue = await _venueContext.Venues
                    .Include(v => v.VenueImages)
                    .Include(o => o.Owner)
                    .Include(vt => vt.VenueType)
                    .Include(s => s.VenueSports).ThenInclude(vs => vs.Sport)
                    .Include(va => va.VenueAvailabilityTimes)
                    .FirstOrDefaultAsync(v => v.Id == id);
                if (venue == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");
                return new VenueResponseDto
                {
                    Id = venue.Id,
                    Name = venue.Name,
                    Street = venue.Street,
                    Number = venue.Number,
                    Complement = venue.Complement,
                    Neighborhood = venue.Neighborhood,
                    City = venue.City,
                    State = venue.State,
                    PostalCode = venue.PostalCode,
                    Capacity = venue.Capacity,
                    Latitude = venue.Latitude,
                    Longitude = venue.Longitude,
                    Description = venue.Description,
                    VenueTypeId = venue.VenueTypeId,
                    Rules = venue.Rules,
                    OwnerId = venue.OwnerId,
                    OwnerName = venue.Owner.FirstName,
                    Sports = venue.VenueSports.Select(s => s.Sport.Name).ToList(),
                    ImageUrls = venue.VenueImages.Select(i => i.ImageUrl).ToList(),
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

    }
}