using CinemaApp.Application.DTO.AuthDTO.PasswordManagement;
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
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieScreening> MovieScreenings { get; set; }
        public DbSet<ScreeningSeat> ScreeningSeats { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── MovieGenre (junction) ──────────────────────────────────────
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // ── Movie ──────────────────────────────────────────────────────
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Screenings)
                .WithOne(s => s.Movie)
                .HasForeignKey(s => s.MovieId);

            // ── MovieScreening ─────────────────────────────────────────────
            // Isti film ne može imati dve projekcije u isto vreme
            modelBuilder.Entity<MovieScreening>()
                .HasIndex(ms => new { ms.MovieId, ms.StartTime })
                .IsUnique();

            // ── ScreeningSeat ──────────────────────────────────────────────
            // Sedište A3 može postojati samo jednom po projekciji
            modelBuilder.Entity<ScreeningSeat>()
                .HasIndex(s => new { s.MovieScreeningId, s.Row, s.Number })
                .IsUnique();

            modelBuilder.Entity<ScreeningSeat>()
                .HasOne(s => s.MovieScreening)
                .WithMany(ms => ms.Seats)
                .HasForeignKey(s => s.MovieScreeningId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kad se rezervacija otkaže/obriše, sedište postaje slobodno (SetNull)
            modelBuilder.Entity<ScreeningSeat>()
                .HasOne(s => s.Reservation)
                .WithMany(r => r.Seats)
                .HasForeignKey(s => s.ReservationId)
                .OnDelete(DeleteBehavior.SetNull);

            // ── Reservation ────────────────────────────────────────────────
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.MovieScreening)
                .WithMany(ms => ms.Reservations)
                .HasForeignKey(r => r.MovieScreeningId)
                .OnDelete(DeleteBehavior.Restrict); // ne brišemo projekciju ako ima rezervacija

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.ReservationCode)
                .IsUnique();

            // ── Rating ─────────────────────────────────────────────────────
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            // Korisnik može oceniti film samo jednom
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.MovieId })
                .IsUnique();

            // ── Genre ──────────────────────────────────────────────────────
            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            // ── User ───────────────────────────────────────────────────────
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // FavoriteGenres many-to-many (automatska junction tabela)
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteGenres)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFavoriteGenres"));

            // ── PasswordResetToken ─────────────────────────────────────────
            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PasswordResetToken>()
                .HasIndex(t => t.Token)
                .IsUnique();
        }
    }
}