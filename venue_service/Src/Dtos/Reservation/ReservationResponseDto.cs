namespace venue_service.Src.Dtos.Reservation
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VenueId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool? IsPaid { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
