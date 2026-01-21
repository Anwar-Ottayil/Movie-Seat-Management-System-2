using Movie_Seat_Management_System_2.DTOs;

namespace Movie_Seat_Management_System_2.Repositories
{
    public interface ISeatRepository
    {
        Guid HoldSeats(Guid showId, int seatCount);
        void ConfirmBooking(Guid holdId);
        void ReleaseExpiredHolds();

        SeatStatusResponse GetSeatStatus(Guid showId);
    }
}
