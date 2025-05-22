namespace venue_service.Src.Dtos.Payment
{
    public class ReservationPaymentStatusDto
    {
        public int ReservationId { get; set; }
        public bool HasPayment { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
