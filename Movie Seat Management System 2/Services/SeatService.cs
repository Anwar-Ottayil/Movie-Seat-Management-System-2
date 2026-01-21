using Movie_Seat_Management_System_2.DTOs;
using Movie_Seat_Management_System_2.Repositories;

namespace Movie_Seat_Management_System_2.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _repository;

        public SeatService(ISeatRepository repository)
        {
            _repository = repository;
        }

        public Guid HoldSeats(Guid showId, int seatCount)
        {
            return _repository.HoldSeats(showId, seatCount);
        }

        public void ConfirmBooking(Guid holdId)
        {
            _repository.ConfirmBooking(holdId);
        }

        public SeatStatusResponse GetSeatStatus(Guid showId)
        {
            return _repository.GetSeatStatus(showId);
        }
    }
}
