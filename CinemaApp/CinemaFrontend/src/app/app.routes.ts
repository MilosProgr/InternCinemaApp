import { Routes } from '@angular/router';
import { guardGuard } from './guards/guard-guard';
import { LoginComponent } from './components/login-component/login-component';
import { GenreComponent } from './components/genre-component/genre-component';
import { MovieComponent } from './components/movie-component/movie-component';
import { MovieScreeningComponent } from './components/movie-screening-component/movie-screening-component';
import { ReservationComponent } from './components/reservation-component/reservation-component';
import { ReservationSeatComponent } from './components/reservation-seat-component/reservation-seat-component';
import { SeatComponent } from './components/seat-component/seat-component';
import { RatingComponent } from './components/rating-component/rating-component';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: "login", component: LoginComponent },
    { path: "Genre", component: GenreComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Movie", component: MovieComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "MovieScreening", component: MovieScreeningComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Reservation", component: ReservationComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "ReservationSeat", component: ReservationSeatComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Seat", component: SeatComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "Rating", component: RatingComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] }
];
