using Microsoft.AspNetCore.Mvc;
using System.Net;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.Reservation;

namespace venue_service.Src.Controllers.Payment
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public PaymentController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPatch("{reservationId}/pay")]
        public async Task<IActionResult> PayReservation(int reservationId, [FromBody] PaymentRequestDto dto)
        {
            var result = await _reservationService.PayReservationAsync(reservationId, dto);
            return Ok(result);
        }
    }
}


