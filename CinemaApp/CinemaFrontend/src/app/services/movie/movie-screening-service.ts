import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { MovieScreening } from '../../models/movie_screening.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ScreeningSeat } from '../../models/screeningSeat';

@Injectable({
  providedIn: 'root'
})
export class MovieScreeningService extends CrudService<MovieScreening> {

    private readonly screeningsUrl = `${environment.baseUrl}/screenings`;

    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/MovieScreening`)
    }



    getUpcoming7Days(
    genreId?: number | null,
    date?: string | null,
    sortBy: string = 'chronologically'
): Observable<MovieScreening[]> {

    let params = new HttpParams().set('sortBy', sortBy);

    if (genreId)
        params = params.set('genreId', genreId.toString());

    if (date)
        params = params.set('date', date);

    const url =
        `${environment.baseUrl}/MovieScreening/upcoming7days`;

    console.log('=== API URL ===');
    console.log(url);
    console.log('=== PARAMS ===');
    console.log(params.toString());

    return this.http.get<MovieScreening[]>(url, { params });
}

    getById(id: number): Observable<MovieScreening> {
        return this.http.get<MovieScreening>(
        `${environment.baseUrl}/MovieScreening/${id}`
        );
    }




    getSeats(screeningId: number): Observable<ScreeningSeat[]> {
        return this.http.get<ScreeningSeat[]>(
        `${this.screeningsUrl}/${screeningId}/seats`
        );
    }
}
