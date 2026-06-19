import { Routes } from '@angular/router';
import { guardGuard } from './guards/guard-guard';
import { LoginComponent } from './components/login-component/login-component';
import { GenreComponent } from './components/genre-component/genre-component';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: "Genre", component: GenreComponent, data: { allowedRoles: ['ADMIN'] }, canActivate: [guardGuard] },
    { path: "login", component: LoginComponent }
];
