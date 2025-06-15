using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;
using venue_service.Src.Interfaces.ReservationInterfaces;
using venue_service.Src.Services.Reservation;

namespace venue_service.Src.Controllers.Reservation
{
    [Authorize]
    [ApiController]
    [Route("api/reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId, [FromQuery] ReservationStatusEnum? status)
        {
            var response = await _reservationService.GetReservationsByUserIdAsync(userId, status);
            return Ok(response);
        }

        [HttpGet("venue/{venueId}")]
        public async Task<IActionResult> GetVenueReservations(int venueId)
        {
            var response = await _reservationService.GetReservationByVenueIdAsync(venueId);
            return Ok(response);
        }

        [HttpPut("pay/{reservationId}")]
        public async Task<IActionResult> PayReservation(int reservationId)
        {
            var response = await _reservationService.PayReservationAsync(reservationId);
            return Ok(response);
        }
    }
}
