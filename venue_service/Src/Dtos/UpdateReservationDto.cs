namespace venue_service.Src.Dtos
{
    public class UpdateReservationDto
    {
        public int UserId { get; set; }
        public int VenueId { get; set; }
        public string Status { get; set; }
        public int VenueAvailabilityTimeId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
