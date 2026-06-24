import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { MovieScreening } from '../../models/movie_screening.model';
import { Movie } from '../../models/movie.model';
import { Genre } from '../../models/genre.model';

import { MovieScreeningService } from '../../services/movie/movie-screening-service';
import { GenreService } from '../../services/genre/genre';

interface GroupedMovie {
    movie: Movie;
    screenings: MovieScreening[];
}

@Component({
    selector: 'app-homepage-component',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './homepage-component.html',
    styleUrl: './homepage-component.css',
})
export class HomepageComponent implements OnInit {

    screenings: MovieScreening[] = [];
    genres: Genre[] = [];
    groupedMovies: GroupedMovie[] = [];
    next7Days: Date[] = [];  // ← property umesto get

    selectedGenreId: number | null = null;
    selectedDate: string | null = null;
    sortBy: 'chronologically' | 'alphabetically' = 'chronologically';

    isLoading = false;
    errorMessage: string | null = null;

    constructor(
        private movieScreeningService: MovieScreeningService,
        private genreService: GenreService,
        private router: Router
    ) {}

    ngOnInit(): void {
        // Generiši datume jednom
        this.next7Days = Array.from({ length: 7 }, (_, i) => {
            const d = new Date();
            d.setDate(d.getDate() + i);
            return d;
        });

        const today = new Date();
        const year = today.getFullYear();
        const month = String(today.getMonth()+1).padStart(2,'0');
        const day = String(today.getDate()).padStart(2,'0');

        this.selectedDate = `${year}-${month}-${day}`;

        this.loadGenres();
        this.loadScreenings();
    }

    loadGenres(): void {
        this.genreService.getAll().subscribe({
            next: (data: any) => {
                this.genres = Array.isArray(data) ? data : (data.items ?? []);
            },
            error: (err) => console.error('Greška pri učitavanju žanrova', err)
        });
    }

    loadScreenings(): void {

    this.isLoading = true;
    this.errorMessage = null;

    console.log('====================');
    console.log('LOAD SCREENINGS');
    console.log('genreId:', this.selectedGenreId);
    console.log('date:', this.selectedDate);
    console.log('sortBy:', this.sortBy);

    this.movieScreeningService
        .getUpcoming7Days(
            this.selectedGenreId,
            this.selectedDate,
            this.sortBy
        )
        .subscribe({

            next: (data: any) => {

                console.log('=== RESPONSE ===');
                console.log(data);

                const list =
                    Array.isArray(data)
                        ? data
                        : (data.items ?? []);

                console.log('=== LIST ===');
                console.log(list);

                this.screenings = list;

                this.groupByMovie(list);

                console.log('=== GROUPED MOVIES ===');
                console.log(this.groupedMovies);

                this.isLoading = false;
            },

            error: (err) => {

                console.error('=== API ERROR ===');
                console.error(err);

                this.errorMessage =
                    'Nije moguće učitati repertoar.';

                this.isLoading = false;
            }
        });
}

    filterChanged(): void {
        this.loadScreenings();
    }

    groupByMovie(screenings: MovieScreening[]): void {

    console.log('groupByMovie input');
    console.log(screenings);

    const map = new Map<number, GroupedMovie>();

    screenings.forEach(s => {
    
        console.log('screening');
        console.log(s);

        console.log('screening');
        console.log(s.movie);

        if (!s.movie) {

            console.log('movie NULL');
            return;
        }

        if (!map.has(s.movieId)) {

            map.set(s.movieId, {
                movie: s.movie,
                screenings: []
            });
        }

        map.get(s.movieId)!.screenings.push(s);
    });

    this.groupedMovies = Array.from(map.values());

    console.log('grouped result');
    console.log(this.groupedMovies);
}

    // ← UTC poređenje da nema timezone pomaka
    getScreeningsForDay(movieScreenings: MovieScreening[], day: Date): MovieScreening[] {
        return movieScreenings.filter(s => {
            const d = new Date(s.startTime);
            return (
                d.getUTCFullYear() === day.getFullYear() &&
                d.getUTCMonth()    === day.getMonth() &&
                d.getUTCDate()     === day.getDate()
            );
        });
    }

    isPast(screening: MovieScreening): boolean {
        return new Date(screening.startTime) < new Date();
    }

    getStars(rating?: number): string {
        if (!rating) return '';
        const full = Math.round(rating);
        return '★'.repeat(full) + '☆'.repeat(5 - full);
    }

    MyReservations() {
        this.router.navigate(['/MyReservation']);
    }

    Reservations() {
        this.router.navigate(['/Reservation']);
    }
}