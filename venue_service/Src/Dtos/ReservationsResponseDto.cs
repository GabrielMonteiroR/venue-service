namespace venue_service.Src.Dtos
{
    public class ReservationsResponseDto
    {
        public string Message { get; set; }
        public List<ReservationResponseDto> Reservations { get; set; }
    }
}
