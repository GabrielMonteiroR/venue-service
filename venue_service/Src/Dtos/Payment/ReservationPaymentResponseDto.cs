namespace venue_service.Src.Dtos.Payment
{
    public class ReservationPaymentResponseDto
    {
        public int ReservationId { get; set; }
        public bool Paid { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
