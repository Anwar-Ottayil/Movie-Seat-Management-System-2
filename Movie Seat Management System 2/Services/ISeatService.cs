using Movie_Seat_Management_System_2.DTOs;

namespace Movie_Seat_Management_System_2.Services
{
    public interface ISeatService
    {
        Guid HoldSeats(Guid showId, int seatCount);
        void ConfirmBooking(Guid holdId);

        SeatStatusResponse GetSeatStatus(Guid showId);
    }
}
