using Dapper;
using Movie_Seat_Management_System_2.DTOs;
using Movie_Seat_Management_System_2.Models;
using System.Data;

namespace Movie_Seat_Management_System_2.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly IDbConnection _db;

        public SeatRepository(IDbConnection db)
        {
            _db = db;
        }

        public Guid HoldSeats(Guid showId, int seatCount)
        {
            if (_db.State != ConnectionState.Open)
                _db.Open();

            using var transaction = _db.BeginTransaction();

            var seats = _db.Query<Seat>(
                @"SELECT TOP (@SeatCount) *
          FROM Seats WITH (UPDLOCK, ROWLOCK)
          WHERE ShowId = @ShowId AND Status = 'AVAILABLE'",
                new { ShowId = showId, SeatCount = seatCount },
                transaction
            ).ToList();

            if (seats.Count < seatCount)
            {
                transaction.Rollback();
                throw new Exception("Not enough seats available");
            }

            var holdId = Guid.NewGuid();
            var expiry = DateTime.UtcNow.AddMinutes(5);

            foreach (var seat in seats)
            {
                _db.Execute(
                    @"UPDATE Seats
              SET Status='HELD',
                  HoldId=@HoldId,
                  HoldExpiresAt=@Expiry
              WHERE SeatId=@SeatId",
                    new { HoldId = holdId, Expiry = expiry, seat.SeatId },
                    transaction
                );
            }

            transaction.Commit();
            return holdId;
        }


        public void ConfirmBooking(Guid holdId)
        {
            if (_db.State != ConnectionState.Open)
                _db.Open();

            var affected = _db.Execute(
                @"UPDATE Seats
          SET Status='BOOKED',
              HoldId=NULL,
              HoldExpiresAt=NULL
          WHERE HoldId=@HoldId
            AND HoldExpiresAt > GETUTCDATE()",
                new { HoldId = holdId }
            );

            if (affected == 0)
                throw new Exception("Invalid or expired hold");
        }
        public void ReleaseExpiredHolds()
        {
            _db.Execute(
                @"UPDATE Seats
                  SET Status='AVAILABLE',
                      HoldId=NULL,
                      HoldExpiresAt=NULL
                  WHERE Status='HELD'
                    AND HoldExpiresAt < GETUTCDATE()"
            );
        }
        public SeatStatusResponse GetSeatStatus(Guid showId)
        {
            if (_db.State != ConnectionState.Open)
                _db.Open();

            var sql = @"
        SELECT
            SUM(CASE WHEN Status = 'AVAILABLE' THEN 1 ELSE 0 END) AS Available,
            SUM(CASE WHEN Status = 'HELD' THEN 1 ELSE 0 END) AS Held,
            SUM(CASE WHEN Status = 'BOOKED' THEN 1 ELSE 0 END) AS Booked
        FROM Seats
        WHERE ShowId = @ShowId";

            var result = _db.QuerySingle<SeatStatusResponse>(sql, new { ShowId = showId });
            result.ShowId = showId;
            return result;
        }
    }
}
