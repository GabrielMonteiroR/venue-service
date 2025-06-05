using Microsoft.EntityFrameworkCore;
using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Sports;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.SportInterfaces;

namespace venue_service.Src.Services.Sport
{
    public class SportService : ISportInterface
    {
        private readonly VenueContext _venueContext;

        public SportService(VenueContext venueContext)
        {
            _venueContext = venueContext;
        }

        public async Task<SportsResponseDto> GetAllSportsAsync()
        {
            try
            {
                var sports = await _venueContext.VenueSports.ToListAsync();
                
                if (sports is null || sports.Count == 0)
                {
                    { throw new HttpResponseException(HttpStatusCode.NoContent, "No sports found", "There are no sports available at the moment."); }
                }

                return new SportsResponseDto
                {
                    Message = "Sports retrieved successfully",
                    Data = sports.Select(s => new SportResponseDto
                    {
                        Id = s.SportId,
                        Name = s.Sport.Name
                    }).ToList()
                };

            } catch (Exception ex)
            {
               throw new HttpResponseException(HttpStatusCode.InternalServerError, "An internal server error ocurred", ex.Message);
            }
    }
}
