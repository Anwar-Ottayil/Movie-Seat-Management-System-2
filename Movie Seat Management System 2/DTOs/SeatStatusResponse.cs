namespace Movie_Seat_Management_System_2.DTOs
{
    public class SeatStatusResponse
    {
        public Guid ShowId { get; set; }
        public int Available { get; set; }
        public int Held { get; set; }
        public int Booked { get; set; }
    }
}
