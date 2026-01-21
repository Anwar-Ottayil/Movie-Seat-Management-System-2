using Microsoft.Data.SqlClient;
using Movie_Seat_Management_System_2.BackgroundJobs;
using Movie_Seat_Management_System_2.Repositories;
using Movie_Seat_Management_System_2.Services;
using Movie_Seat_Management_System_2.BackgroundJobs;
using Movie_Seat_Management_System_2.Repositories;
using Movie_Seat_Management_System_2.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

builder.Services.AddHostedService<SeatHoldCleanupService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
