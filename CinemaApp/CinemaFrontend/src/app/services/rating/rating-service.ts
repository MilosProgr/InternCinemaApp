import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { Rating } from '../../models/rating.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RatingService extends CrudService<Rating> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/Rating`)
    }

    rate(movieId: number, stars: number): Observable<Rating> {
    return this.http.post<Rating>(`${environment.baseUrl}/Rating`, { movieId, stars });
  }
}
