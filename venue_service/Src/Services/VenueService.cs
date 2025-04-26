using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;
using venue_service.Src.Models;

namespace venue_service.Src.Services
{
    public class VenueService : IVenueService
    {
        private readonly DatabaseContext _context;

        public VenueService(DatabaseContext context)
        {
            _context = context;
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

                if (venue is null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid data", "The provided venue data is null.");

                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();

                return new VenueResponseDto
                {
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
                if (venue == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Not Found", "Venue not found");
                }

                venue.Name = dto.Name;
                venue.Address = dto.Address;
                venue.Capacity = dto.Capacity;
                venue.Latitude = dto.Latitude;
                venue.Longitude = dto.Longitude;
                venue.Description = dto.Description;
                venue.AllowLocalPayment = dto.AllowLocalPayment;
                venue.VenueTypeId = dto.VenueTypeId;
                venue.Rules = dto.Rules;

                await _context.SaveChangesAsync(); 

                return new VenueResponseDto
                {
                    Name = venue.Name,
                    Address = venue.Address,
                    Capacity = venue.Capacity,
                    Latitude = venue.Latitude,
                    Longitude = venue.Longitude,
                    Description = venue.Description,
                    AllowLocalPayment = venue.AllowLocalPayment,
                    VenueTypeId = venue.VenueTypeId,
                    Rules = venue.Rules,
                };

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }
    }
}
