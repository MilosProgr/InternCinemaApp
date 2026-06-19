import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, catchError, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { User } from '../../models/user.model';
import { Token } from '../../models/token';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private baseUrl = environment.baseUrl;

  token: string | null = null;
  user: any = null;

  rolesSubject: BehaviorSubject<Set<string>> =
    new BehaviorSubject<Set<string>>(new Set());

  loggedOut = false;
  loggedIn = false;

  constructor(
    private client: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {

    // Učitavanje tokena nakon refresh-a
    if (isPlatformBrowser(this.platformId)) {

      const savedToken = localStorage.getItem("token");

      if (savedToken) {
        try {

          const decodedToken = JSON.parse(
            atob(savedToken.split(".")[1])
          );

          this.token = savedToken;
          this.user = decodedToken;
          this.loggedIn = true;

          const role =
            decodedToken[
              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            ];

          this.rolesSubject.next(
            new Set([role])
          );

          console.log("Token obnovljen:", decodedToken);

        } catch (e) {
          console.error(
            "Greška pri učitavanju tokena:",
            e
          );

          this.logout();
        }
      }
    }
  }


  login(user: User) {

    console.log(
      "Pokušaj prijave:",
      user
    );

    return this.client
      .post<Token>(
        `${this.baseUrl}/Auth/login`,
        user
      )
      .pipe(

        tap((response: Token) => {

          console.log(
            "Dobijen token:",
            response
          );


          if (response && response.token) {

            try {

              const decodedToken = JSON.parse(
                atob(response.token.split(".")[1])
              );


              localStorage.setItem(
                "token",
                response.token
              );


              this.token = response.token;
              this.user = decodedToken;
              this.loggedIn = true;
              this.loggedOut = false;


              const role =
                decodedToken[
                  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                ];


              this.rolesSubject.next(
                new Set([role])
              );


              console.log(
                "Dekodirani JWT:",
                decodedToken
              );


            } catch(e) {

              console.error(
                "Greška dekodiranja tokena:",
                e
              );

            }

          }

        }),


        catchError(error => {

          console.error(
            "Login greška:",
            error
          );

          return of(null);

        })

      );
  }



  logout(): void {

    this.token = null;
    this.user = null;

    this.loggedIn = false;
    this.loggedOut = true;

    this.rolesSubject.next(
      new Set()
    );


    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem("token");
    }

  }



  validateRoles(roles: string[]): boolean {

    if (!this.user) {
      return false;
    }


    const userRole =
      this.user[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];


    return roles.includes(userRole);
  }


  getCurrentRole(): string | null {

    if (!this.user) {
      return null;
    }


    return this.user[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];

  }

}