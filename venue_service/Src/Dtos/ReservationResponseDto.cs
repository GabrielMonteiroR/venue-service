namespace venue_service.Src.Dtos
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime CreatedAt { get; set; }

        public int VenueId { get; set; }
        public int UserId { get; set; }
        public int LocationAvailabilityTimeId { get; set; }
    }
}
