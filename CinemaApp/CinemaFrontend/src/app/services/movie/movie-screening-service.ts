import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { MovieScreening } from '../../models/movie_screening.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ScreeningSeat } from '../../models/screeningSeat';

@Injectable({
  providedIn: 'root'
})
export class MovieScreeningService extends CrudService<MovieScreening> {


    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/MovieScreening`)
    }



    getUpcoming7Days(
        genreId:number | null,
        date:string | null,
        sortBy:string
    ): Observable<MovieScreening[]> {

        let params = '';

        if(genreId){
            params += `genreId=${genreId}&`;
        }

        if(date){
            params += `date=${date}&`;
        }

        params += `sortBy=${sortBy}`;


        return this.http.get<MovieScreening[]>(
            `${this.baseUrl}/upcoming7days?${params}`
        );
    }



    getSeats(id:number): Observable<ScreeningSeat[]> {

        return this.http.get<ScreeningSeat[]>(
            `${environment.baseUrl}/screenings/${id}/seats`
        );

    }
}
