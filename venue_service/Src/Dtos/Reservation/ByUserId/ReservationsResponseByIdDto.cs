using venue_service.Src.Dtos.Reservation.ByUserId;

namespace venue_service.Src.Dtos.Reservation.ByUserId
{
    public class ReservationsResponseByIdDto
    {
        public string Message { get; set; }
        public List<ReservationResponseByIdDto> Reservations { get; set; }
    }
}
