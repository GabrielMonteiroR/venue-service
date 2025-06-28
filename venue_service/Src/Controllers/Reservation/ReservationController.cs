using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Interfaces.ReservationInterfaces;
using venue_service.Src.Services.Reservation;

namespace venue_service.Src.Controllers.Reservation
{
    [Authorize]
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            var response = await _reservationService.CreateReservationAsync(dto);
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId)
        {
            var response = await _reservationService.GetReservationsByUserIdAsync(userId);
            return Ok(response);
        }

        [HttpGet("by-venue/{venueId}")]
        public async Task<IActionResult> GetVenueReservations(int venueId)
        {
            var response = await _reservationService.GetReservationByVenueIdAsync(venueId);
            return Ok(response);
        }

        [HttpPut("pay-reservation/{reservationId}")]
        public async Task<IActionResult> PayReservation(int reservationId)
        {
            var response = await _reservationService.PayReservationAsync(reservationId);
            return Ok(response);
        }

        [HttpGet("history/by-venue/{venueId}")]
        public async Task<IActionResult> GetHistoryByUserIdAsync(int venueId)
        {
            var response = await _reservationService.GetHistoryByUserIdAsync(venueId);
            return Ok(response);
        }

        [HttpGet("history/by-user/{userId}")]
        public async Task<IActionResult> GetHistoryByVenueIdAsync(int userId)
        {
            var response = await _reservationService.GetHistoryByVenueId(userId);
            return Ok(response);
        }

        [HttpGet("next-reservation/{userId}")]
        public async Task<IActionResult> GetNextUserReservationAsync(int userId)
        {
            var response = await _reservationService.GetNextUserReservationAsync(userId);
            return Ok(response);
        }
    }
}
