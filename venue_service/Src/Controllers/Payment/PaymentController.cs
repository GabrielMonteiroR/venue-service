using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Contexts;
using venue_service.Src.Interfaces.Payment;
using venue_service.Src.Models.Payment;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ReservationContext _reservationContext;

    public PaymentController(IPaymentService paymentService, ReservationContext reservationContext)
    {
        _paymentService = paymentService;
        _reservationContext = reservationContext;
    }

    [HttpPatch("{reservationId}/pay")]
    public async Task<IActionResult> PayReservation(int reservationId, [FromBody] PaymentRequestDto dto)
    {
        var reservation = await _reservationContext.Reservations
            .Include(r => r.PaymentRecord)
            .Include(r => r.Venue)
            .Include(r => r.VenueAvailabilityTimeId)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
            return NotFound("Reserva não encontrada");

        if (reservation.PaymentRecord != null)
            return BadRequest("Esta reserva já possui pagamento registrado.");

        var availability = await _reservationContext.VenueAvailabilities.FindAsync(reservation.VenueAvailabilityTimeId);

        var (status, mercadoPagoId) = await _paymentService.CreatePaymentAsync(
            dto.UserEmail,
            dto.CardToken,
            availability.Price,
            $"Pagamento reserva #{reservationId}");

        var paymentRecord = new PaymentRecordEntity
        {
            ReservationId = reservationId,
            Amount = availability.Price,
            Status = status,
            MercadoPagoPaymentId = mercadoPagoId,
            CreatedAt = DateTime.UtcNow,
            PaidAt = status == "approved" ? DateTime.UtcNow : null
        };

        _reservationContext.PaymentRecords.Add(paymentRecord);
        await _reservationContext.SaveChangesAsync();

        return Ok(new
        {
            status,
            mercadoPagoId,
            paid = status == "approved"
        });
    }
}
