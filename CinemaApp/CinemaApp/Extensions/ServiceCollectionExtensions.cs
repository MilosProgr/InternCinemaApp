using CinemaApp.Application.Services.Auth;
using CinemaApp.Application.Services.Genres;
using CinemaApp.Application.Services.MovieGenres;
using CinemaApp.Application.Services.Movies;
using CinemaApp.Application.Services.MovieScreenings;
using CinemaApp.Application.Services.Ratings;
using CinemaApp.Application.Services.Reservations;
using CinemaApp.Application.Services.ScreeningSeats;
using CinemaApp.Application.Services.Users;
using CinemaApp.Infrastructure.Services;
using CinemaApp.Services.Genres;
using CinemaApp.Services.Movies;
using CinemaApp.Services.MovieScreenings;
using CinemaApp.Services.Ratings;
using CinemaApp.Services.Reservations;

using CinemaApp.Services.Users;

namespace CinemaApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IGenreService, GenreService>();

            services.AddScoped<IMovieService, MovieService>();

            services.AddScoped<IMovieScreeningService, MovieScreeningService>();

            services.AddScoped<IMovieGenreService, MovieGenreService>();

            services.AddScoped<IReservationService, ReservationService>();

            services.AddScoped<IScreeningSeatService, ScreeningSeatService>();

            services.AddScoped<IRatingService, RatingService>();

       


            return services;
        }
    }
}
