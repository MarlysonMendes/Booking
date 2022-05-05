using CwkBooking.Api;
using CwkBooking.Api.Middleware;
using CwkBooking.Dal;
using CwkBooking.Dal.Repositories;
using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Abstractions.Services;
using CwkBooking.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataSource>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IHotelsRepository,HotelRepository>();
builder.Services.AddScoped<IRoomsRepository, RoomRepository>();
builder.Services.AddScoped<IReservationsRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>( options =>
{
    options.UseSqlServer(cs);
});

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
