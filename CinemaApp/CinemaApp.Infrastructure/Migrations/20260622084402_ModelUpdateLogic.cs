using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CinemaApp.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdateLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_GenreId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_MovieScreenings_MovieScreeningId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservationSeats");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_MovieScreenings_MovieId",
                table: "MovieScreenings");

            migrationBuilder.DropIndex(
                name: "IX_Movies_GenreId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieScreeningId = table.Column<int>(type: "integer", nullable: false),
                    Row = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    IsOccupied = table.Column<bool>(type: "boolean", nullable: false),
                    ReservationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreeningSeats_MovieScreenings_MovieScreeningId",
                        column: x => x.MovieScreeningId,
                        principalTable: "MovieScreenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreeningSeats_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteGenres",
                columns: table => new
                {
                    FavoriteGenresId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteGenres", x => new { x.FavoriteGenresId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteGenres_Genres_FavoriteGenresId",
                        column: x => x.FavoriteGenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteGenres_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_MovieId",
                table: "Ratings",
                columns: new[] { "UserId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieScreenings_MovieId_StartTime",
                table: "MovieScreenings",
                columns: new[] { "MovieId", "StartTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningSeats_MovieScreeningId_Row_Number",
                table: "ScreeningSeats",
                columns: new[] { "MovieScreeningId", "Row", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningSeats_ReservationId",
                table: "ScreeningSeats",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteGenres_UserId",
                table: "UserFavoriteGenres",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_MovieScreenings_MovieScreeningId",
                table: "Reservations",
                column: "MovieScreeningId",
                principalTable: "MovieScreenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_MovieScreenings_MovieScreeningId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "ScreeningSeats");

            migrationBuilder.DropTable(
                name: "UserFavoriteGenres");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_MovieId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_MovieScreenings_MovieId_StartTime",
                table: "MovieScreenings");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Movies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "ReservationSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservationId = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationSeats_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Row = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieScreenings_MovieId",
                table: "MovieScreenings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSeats_ReservationId",
                table: "ReservationSeats",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_GenreId",
                table: "Movies",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_MovieScreenings_MovieScreeningId",
                table: "Reservations",
                column: "MovieScreeningId",
                principalTable: "MovieScreenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
