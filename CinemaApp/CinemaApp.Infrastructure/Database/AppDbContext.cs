//using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<MovieScreening> MovieScreenings { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<ReservationSeat> ReservationSeats { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Seat> Seats { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movie>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.GenreId);



            modelBuilder.Entity<MovieScreening>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.Screenings)
                .HasForeignKey(x => x.MovieId);



            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);



            modelBuilder.Entity<ReservationSeat>()
                .HasOne(x => x.Reservation)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Rating>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.MovieId);



            modelBuilder.Entity<Rating>()
                .HasOne(x => x.User)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.UserId);



            modelBuilder.Entity<Genre>()
                .HasIndex(x => x.Name)
                .IsUnique();



            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();


            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();



            modelBuilder.Entity<Reservation>()
                .HasIndex(x => x.ReservationCode)
                .IsUnique();

        }



    }
}
