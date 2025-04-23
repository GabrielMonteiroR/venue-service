using Microsoft.EntityFrameworkCore;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;

namespace venue_service.Src.Services
{
    public class VenueTypeService : IVenueType
    {
        private readonly DatabaseContext _context;

        public VenueTypeService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<VenueTypesResponseDto> GetAllVenuesTypes()
        {
            try
            {
                var venueTypes = await _context.VenueType.ToListAsync();

                if (venueTypes is null || !venueTypes.Any())
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NoContent, "No Venue Types Found", "No venue types were found in the database.");
                }

            } catch(Exception ex)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }
    }
}
