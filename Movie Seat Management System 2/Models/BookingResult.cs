namespace Movie_Seat_Management_System_2.Models
{
    public class BookingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? HoldId { get; set; }
    }
}
