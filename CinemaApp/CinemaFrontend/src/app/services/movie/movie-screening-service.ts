import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { MovieScreening } from '../../models/movie_screening.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieScreeningService extends CrudService<MovieScreening> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/MovieScreening`)
    }
}
