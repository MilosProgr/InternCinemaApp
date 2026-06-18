import { Inject, PLATFORM_ID, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, catchError, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { User } from '../../models/user.model';
import { Token } from '../../models/token';

@Service()
export class LoginService {
    private baseUrl = environment.baseUrl;

  token: string | null = null;
  user: any = null;
  rolesSubject: BehaviorSubject<Set<string>> = new BehaviorSubject<Set<string>>(new Set([]));
  loggedOut = false;
  loggedIn = false;

  constructor(private client: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) { }

  login(user: User) {
    console.log('Pokušaj prijave sa korisnikom:', user);
    return this.client.post<Token>(`${this.baseUrl}/login`, user).pipe(
      tap((token: Token) => {
        console.log('Dobijen token:', token);
        if (token && token.token) {
          try {
            const decodedToken = JSON.parse(atob(token.token.split(".")[1]));
            localStorage.setItem("token", token.token);
            console.log('Dekodirani token:', decodedToken);
            this.user = decodedToken;
            this.loggedIn = true;
            console.log('Podaci korisnika:', this.user);
          } catch (e) {
            console.error('Greška prilikom dekodiranja tokena:', e);
          }
        }
      }),
      catchError(error => {
        console.error('Greška prilikom prijave:', error);
        return of(null);
      })
    );
  }

  logout(): void {
    this.token = null;
    this.user = null;
    this.rolesSubject.next(new Set<string>([]));
    this.loggedOut = true;
    localStorage.removeItem("token");
  }

  validateRoles(roles: string[]): boolean {
    if (this.user) {
      const userRoles = new Set(this.user.roles || []);
      return roles.some(role => userRoles.has(role));
    }
    return false;
  }
}
