using CinemaApp.Database;
using CinemaApp.Services.Users;
using CinemaApp.Services.Genres;
using CinemaApp.Services.Movies;
using CinemaApp.Services.MovieScreenings;
using CinemaApp.Services.Reservations;
using CinemaApp.Services.ReservationSeats;
using CinemaApp.Services.Ratings;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Services.Seats;


var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});


// Services

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddScoped<IMovieScreeningService, MovieScreeningService>();

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IReservationSeatService, ReservationSeatService>();

builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddScoped<ISeatService, SeatService>();

// JWT kasnije
// builder.Services.AddAuthentication();



var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}



app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();


app.Run();