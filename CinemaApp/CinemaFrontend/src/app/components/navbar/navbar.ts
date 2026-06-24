import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginService } from '../../services/login/login';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent {

  constructor(
    public loginService: LoginService,
    private router: Router
  ) {}

  get isLoggedIn(): boolean {
    return this.loginService.loggedIn;
  }

  get isAdmin(): boolean {
    return this.loginService.validateRoles(['ADMIN']);
  }

  get username(): string {
    const u = this.loginService.user;
    if (!u) return '';
    // JWT claim za ime
    return u['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      ?? u['unique_name']
      ?? u['sub']
      ?? '';
  }

  logout(): void {
    this.loginService.logout();
    this.router.navigate(['/']);
  }
}