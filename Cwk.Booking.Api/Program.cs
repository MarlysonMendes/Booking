using CwkBooking.Api;
using CwkBooking.Api.Middleware;
using CwkBooking.Dal;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataSource>();
builder.Services.AddAutoMapper(typeof(Program));

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
//app.UseDateTimeHeader();

app.MapControllers();

app.Run();
