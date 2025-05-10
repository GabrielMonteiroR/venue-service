using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;
using venue_service.Src.Models;
using venue_service.Src.Services.ImageService;

namespace venue_service.Src.Services
{
    public class VenueService : IVenueService
    {
        private readonly DatabaseContext _context;
        private readonly IStorageService _storageService;

        public VenueService(DatabaseContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto)
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
                    OwnerId = dto.OwnerId
                };

                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();

                if (dto.Images != null && dto.Images.Any())
                {
                    var urls = await _storageService.UploadVenueImagesAsync(dto.Images);
                    var venueImages = urls.Select(url => new VenueImage
                    {
                        VenueId = venue.Id,
                        ImageURL = url
                    }).ToList();

                    _context.VenueImages.AddRange(venueImages);
                    await _context.SaveChangesAsync();
                }

                var images = await _context.VenueImages
                    .Where(img => img.VenueId == venue.Id)
                    .Select(img => img.ImageURL)
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
                    Images = images
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task<VenuesResponseDto> ListVenuesAsync()
        {
            try
            {
                var venues = await _context.Venues.Include(v => v.VenueImages).ToListAsync();

                if (venues.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");
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
                    AllowLocalPayment = v.AllowLocalPayment,
                    VenueTypeId = v.VenueTypeId,
                    Rules = v.Rules,
                    OwnerId = v.OwnerId,
                    Images = v.VenueImages?.Select(i => i.ImageURL).ToList() ?? new List<string>()
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

        public async Task<VenuesResponseDto> DeleteVenuesAsync(int[] ids)
        {
            try
            {
                var venues = await _context.Venues
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
                        var parsed = _storageService.ParseSupabaseUrl(image.ImageURL);
                        if (parsed != null)
                            await _storageService.DeleteImageAsync(parsed.Value.Bucket, parsed.Value.Path);
                    }
                }

                _context.VenueImages.RemoveRange(venues.SelectMany(v => v.VenueImages));
                _context.Venues.RemoveRange(venues);
                await _context.SaveChangesAsync();

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
                        Images = v.VenueImages?.Select(i => i.ImageURL).ToList() ?? new List<string>()
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
                var venue = await _context.Venues.Include(v => v.VenueImages).FirstOrDefaultAsync(v => v.Id == id);
                if (venue is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");

                var owner = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.OwnerId);
                if (owner is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Owner not found");

                if (owner.Id != venue.OwnerId)
                    throw new HttpResponseException(HttpStatusCode.Forbidden, "Forbidden", "You are not allowed to update this venue");

                venue.Name = dto.Name;
                venue.Address = dto.Address;
                venue.Capacity = dto.Capacity;
                venue.Latitude = dto.Latitude;
                venue.Longitude = dto.Longitude;
                venue.Description = dto.Description;
                venue.AllowLocalPayment = dto.AllowLocalPayment;
                venue.VenueTypeId = dto.VenueTypeId;
                venue.Rules = dto.Rules;

                if (dto.Images != null && dto.Images.Any())
                {
                    foreach (var img in venue.VenueImages)
                    {
                        var parsed = _storageService.ParseSupabaseUrl(img.ImageURL);
                        if (parsed != null)
                            await _storageService.DeleteImageAsync(parsed.Value.Bucket, parsed.Value.Path);
                    }

                    _context.VenueImages.RemoveRange(venue.VenueImages);

                    var newImages = new List<VenueImage>();
                    foreach (var image in dto.Images)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var url = await _storageService.UploadImageAsync(image, "venue-images", fileName);
                        if (url is not null)
                        {
                            newImages.Add(new VenueImage
                            {
                                VenueId = venue.Id,
                                ImageURL = url
                            });
                        }
                    }

                    _context.VenueImages.AddRange(newImages);
                }

                await _context.SaveChangesAsync();

                var updatedImages = await _context.VenueImages
                    .Where(img => img.VenueId == venue.Id)
                    .Select(img => img.ImageURL)
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
                    Images = updatedImages
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
                var owner = await _context.Users.FirstOrDefaultAsync(o => o.Id == id);
                if (owner is null)
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Owner not found");

                var venues = await _context.Venues
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
                    Images = v.VenueImages?.Select(i => i.ImageURL).ToList() ?? new List<string>()
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
    }
}