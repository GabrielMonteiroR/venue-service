using AutoMapper;
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

        public async Task<VenueResponseDto> CreateVenueAsync(CreateVenueDto dto)
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

                var venueDtos = _mapper.Map<List<VenueResponseDto>>(venues);

                return new VenuesResponseDto
                {
                    Message = "Venues retrieved successfully",
                    Data = venueDtos
                };
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }
        public async Task<VenueResponseDto> DeleteVenuesAsync(int[] ids)
        {
            try
            {
                var venues = await _context.Venues.SelectMany(v => ids, (v, id) => new { v, id }).Where(v => v.v.Id == v.id).ToListAsync();

                if (venues.Count == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent, "No Content", "No venues found");
                }

                _context.Venues.RemoveRange(venues.Select(v => v.v));
                await _context.SaveChangesAsync();

                return _mapper.Map<VenueResponseDto>(venues.First().v);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }
    }
}
