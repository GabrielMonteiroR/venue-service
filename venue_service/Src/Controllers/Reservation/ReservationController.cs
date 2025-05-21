using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Interfaces.Reservation;

namespace venue_service.Src.Controllers.Reservation
{
    [ApiController]
    [Route("api/reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize]
        [HttpPost("reservations")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            await _reservationService.CreateReservationAsync(dto);
            return Ok();
        }
    }
}
