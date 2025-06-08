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

        public VenueService(VenueContext context, IStorageService storageService) 
        {
            _venueContext = context;
            _storageService = storageService;
        }

        public async Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto)
        {
            try
            {
                var venue = new VenueEntity
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
                    Address = venue.Address,
                    Capacity = venue.Capacity,
                    Latitude = venue.Latitude,
                    Longitude = venue.Longitude,
                    Description = venue.Description,
                    AllowLocalPayment = venue.AllowLocalPayment,
                    VenueTypeId = venue.VenueTypeId,
                    Rules = venue.Rules,
                    OwnerId = venue.OwnerId,
                    ImageUrls = venue.VenueImages.Select(i => i.ImageUrl).ToList()
                };
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Error while registering the venue.", "An unexpected error occurred while creating the venue.", DateTime.UtcNow.ToString());
            }
        }


        public async Task<VenuesResponseDto> GetVenuesAsync(int? venueTypeId = null, DateTime? from = null, DateTime? to = null, int? minCapacity = null, int? maxCapacity = null, string? name = null, List<int>? sportId = null, bool? isReserved = null)
        {
            try
            {
                var query = _venueContext.Venues
                    .Include(v => v.VenueImages)
                    .Include(o => o.Owner)
                    .Include(vt => vt.VenueType)
                    .Include(s => s.VenueSports).ThenInclude(vs => vs.Sport)
                    .Include(va => va.VenueAvailabilityTimes)
                    .AsQueryable();

                if (venueTypeId.HasValue)
                    query = query.Where(v => v.VenueTypeId == venueTypeId.Value);

                if(isReserved.HasValue)
                {
                    query = query.Where(v => v.VenueAvailabilityTimes.Any(va => va.IsReserved == isReserved.Value));
                }

                if (sportId != null && sportId.Count > 0)
                {
                    query = query.Where(v => v.VenueSports.Any(vs => sportId.Contains(vs.SportId)));
                };

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

                if (venues.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found matching the filters.");
                }

                var venueDtos = venues.Select(v => new VenueResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Address = v.Address,
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
                        UserId = t.UserId 
                    }).ToList(),
                    Sports = v.VenueSports.Select(s => s.Sport.Name).ToList(),
                    AllowLocalPayment = v.AllowLocalPayment,
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
                        Name = v.Name,
                        Address = v.Address,
                        Capacity = v.Capacity,
                        Latitude = v.Latitude,
                        Longitude = v.Longitude,
                        Description = v.Description,
                        AllowLocalPayment = v.AllowLocalPayment,
                        VenueTypeId = v.VenueTypeId,
                        Rules = v.Rules,
                        OwnerId = v.OwnerId,
                        ImageUrls = v.VenueImages?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task<VenueResponseDto> UpdateVenueAsync(int id, UpdateVenueRequestDto dto)
        {
            try
            {
                var venue = await _venueContext.Venues.FindAsync(id);

                if (venue is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found", "The specified venue could not be located.");

                venue.Name = dto.Name;
                venue.Address = dto.Address;
                venue.Capacity = dto.Capacity;
                venue.Latitude = dto.Latitude;
                venue.Longitude = dto.Longitude;
                venue.Description = dto.Description;
                venue.AllowLocalPayment = dto.AllowLocalPayment;
                venue.VenueTypeId = dto.VenueTypeId;
                venue.Rules = dto.Rules;

                await _venueContext.SaveChangesAsync();

                var imageUrls = await _venueContext.VenueImages
                    .Where(img => img.VenueId == venue.Id)
                    .Select(img => img.ImageUrl)
                    .ToListAsync();

                return new VenueResponseDto
                {
                    Id = venue.Id,
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
                    ImageUrls = imageUrls
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
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
                    .Where(v => v.OwnerId == id).ToListAsync();

                if (venues.Count == 0) 
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");

                var venueDtos = venues.Select(v => new VenueResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Address = v.Address,
                    Capacity = v.Capacity,
                    Latitude = v.Latitude,
                    Longitude = v.Longitude,
                    Description = v.Description,
                    AllowLocalPayment = v.AllowLocalPayment,
                    VenueTypeId = v.VenueTypeId,
                    Rules = v.Rules,
                    OwnerId = v.OwnerId,
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

    }
}