using Movie_Seat_Management_System_2.Repositories;

namespace Movie_Seat_Management_System_2.BackgroundJobs
{
    public class SeatHoldCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SeatHoldCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();

                repo.ReleaseExpiredHolds();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
