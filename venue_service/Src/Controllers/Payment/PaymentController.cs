using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Payment;
using venue_service.Src.Interfaces.ReservationInterfaces;

namespace venue_service.Src.Controllers.Payment
{
    [Authorize]
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
        public async Task<IActionResult> PayReservation(int reservationId)
        {
            var result = await _reservationService.PayReservationAsync(reservationId);
            return Ok(result);
        }
    }
}


