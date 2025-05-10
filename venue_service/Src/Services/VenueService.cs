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

                var images = new List<VenueImage>();

                foreach (var image in dto.Images)
                {
                    var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var url = await _storageService.UploadImageAsync(image, "venue-images", fileName);

                    if (url != null)
                    {
                        images.Add(new VenueImage
                        {
                            VenueId = venue.Id,
                            ImageURL = url
                        });
                    }
                }

                if (images.Any())
                {
                    _context.VenueImages.AddRange(images);
                    await _context.SaveChangesAsync();
                }

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
                    OwnerId = venue.OwnerId
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
                var venues = await _context.Venues.ToListAsync();

                if (venues.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");
                }

                var venueDtos = venues.Select(v => new VenueResponseDto
                {
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
                }).ToList();

                if (venueDtos.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");
                }

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
                var venues = await _context.Venues.Where(v => ids.Contains(v.Id)).ToListAsync();

                if (venues.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");
                }

                var deletedVenues = venues;

                _context.Venues.RemoveRange(venues);
                await _context.SaveChangesAsync();

                return new VenuesResponseDto
                {
                    Message = "Venues deleted successfully",
                    Data = deletedVenues.Select(v => new VenueResponseDto
                    {
                        Name = v.Name,
                        Address = v.Address,
                        Capacity = v.Capacity,
                        Latitude = v.Latitude,
                        Longitude = v.Longitude,
                        Description = v.Description,
                        AllowLocalPayment = v.AllowLocalPayment,
                        VenueTypeId = v.VenueTypeId,
                        Rules = v.Rules,
                        OwnerId = v.OwnerId
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
                var venue = await _context.Venues.FirstOrDefaultAsync(v => v.Id == id);
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
                    var existingImages = await _context.VenueImages
                        .Where(i => i.VenueId == venue.Id)
                        .ToListAsync();

                    foreach (var img in existingImages)
                    {
                        var parsed = _storageService.ParseSupabaseUrl(img.ImageURL);
                        if (parsed != null)
                        {
                            await _storageService.DeleteImageAsync(parsed.Value.Bucket, parsed.Value.Path);
                        }
                    }

                    _context.VenueImages.RemoveRange(existingImages);

                    var newImages = new List<VenueImage>();
                    foreach (var image in dto.Images)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var url = await _storageService.UploadImageAsync(image, "venue-images", fileName);
                        if (url != null)
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
                    OwnerId = venue.OwnerId
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
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Owner not found");
                }

                var venues = await _context.Venues.Where(v => v.OwnerId == id).ToListAsync();
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
                    OwnerId = v.OwnerId
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
