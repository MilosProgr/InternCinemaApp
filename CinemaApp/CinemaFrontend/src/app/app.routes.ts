import { Routes } from '@angular/router';
import { guardGuard } from './guards/guard-guard';
import { LoginComponent } from './components/login-component/login-component';
import { GenreComponent } from './components/genre-component/genre-component';
import { MovieComponent } from './components/movie-component/movie-component';
import { MovieScreeningComponent } from './components/movie-screening-component/movie-screening-component';
import { ReservationComponent } from './components/reservation-component/ReservationPage/reservation-component';
import { MyReservationsComponent } from './components/reservation-component/MyReservations/my-reservations-component';

import { RatingComponent } from './components/rating-component/rating-component';
import { MovieGenreComponent } from './components/movie-genre-component/movie-genre-component';
import { HomepageComponent } from './components/homepage-component/homepage-component';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: "login", component: LoginComponent },

    { path: "my-reservations", component: MyReservationsComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "homepage", component: HomepageComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },


    { path: "Genre", component: GenreComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Movie", component: MovieComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "MovieScreening", component: MovieScreeningComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "MovieGenres", component: MovieGenreComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Reservation", component: ReservationComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Rating", component: RatingComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] }
];
