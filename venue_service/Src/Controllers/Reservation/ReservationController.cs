using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Reservation;
using venue_service.Src.Enums;
using venue_service.Src.Interfaces.ReservationInterfaces;

namespace venue_service.Src.Controllers.Reservation
{
    [Authorize]
    [ApiController]
    [Route("api/reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        //[Authorize]
        //[HttpPost("reservations")]
        //public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        //{
        //    await _reservationService.CreateReservationAsync(dto);
        //    return Ok();
        //}

        [HttpGet("payment-status/{reservationId}")]
        public async Task<IActionResult> GetPaymentStatus(int reservationId)
        {
            var result = await _reservationService.GetPaymentStatusAsync(reservationId);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId, [FromQuery] ReservationStatusEnum? status)
        {
            var response = await _reservationService.GetReservationsByUserIdAsync(userId, status);
            return Ok(response);
        }


    }
}
