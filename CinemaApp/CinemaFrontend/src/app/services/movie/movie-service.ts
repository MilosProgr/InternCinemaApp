import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { Movie } from '../../models/movie.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})export class MovieService extends CrudService<Movie> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/Movie`)
    }
}
