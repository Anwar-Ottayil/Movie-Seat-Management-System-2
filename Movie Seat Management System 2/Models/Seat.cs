namespace Movie_Seat_Management_System_2.Models
{

    public class Seat
    {
        public Guid SeatId { get; set; }
        public Guid ShowId { get; set; }
        public int SeatNumber { get; set; }



        public string Status { get; set; } = string.Empty;

        public Guid? HoldId { get; set; }
        public DateTime? HoldExpiresAt { get; set; }
    }
}
