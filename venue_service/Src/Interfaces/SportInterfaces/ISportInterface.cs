using venue_service.Src.Dtos.Sports;

namespace venue_service.Src.Interfaces.SportInterfaces
{
    public interface ISportInterface
    {
        Task<SportsResponseDto> GetAllSportsAsync();
    }
}
