using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Seat_Management_System_2.Models;
using Movie_Seat_Management_System_2.Services;

namespace Movie_Seat_Management_System_2.Controllers
{
    [ApiController]
    [Route("api/seats")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _service;

        public SeatsController(ISeatService service)
        {
            _service = service;
        }

        [HttpPost("hold")]
        public IActionResult HoldSeats(HoldRequest request)
        {
            var holdId = _service.HoldSeats(request.ShowId, request.SeatCount);
            return Ok(new { HoldId = holdId });
        }

        [HttpPost("confirm/{holdId}")]
        public IActionResult Confirm(Guid holdId)
        {
            _service.ConfirmBooking(holdId);
            return Ok("Booking confirmed");
        }

        [HttpGet("status/{showId}")]
        public IActionResult GetSeatStatus(Guid showId)
        {
            var status = _service.GetSeatStatus(showId);
            return Ok(status);
        }
    }
}
