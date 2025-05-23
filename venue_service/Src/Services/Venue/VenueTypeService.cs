using Microsoft.EntityFrameworkCore;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.VenueInterfaces;

namespace venue_service.Src.Services.Venue
{
    public class VenueTypeService : IVenueType
    {
        private readonly VenueContext _venueContext;

        public VenueTypeService(VenueContext context)
        {
            _venueContext = context;
        }

        public async Task<VenueTypesResponseDto> GetAllVenueTypes()
        {
            try
            {
                var venueTypes = await _venueContext.VenueTypes.ToListAsync();

                if (venueTypes is null || !venueTypes.Any())
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NoContent, "No Venue Types Found", "No venue types were found in the database.");
                }

                return new VenueTypesResponseDto
                {
                    Message = "Venue Types Found",
                    venueTypesList = venueTypes.Select(v => new VenueTypeResponseDto
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Description = v.Description
                    }).ToList()
                };

            } catch(Exception ex)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }
    }
}
