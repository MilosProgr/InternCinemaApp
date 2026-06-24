import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { MovieGenre } from '../../models/movie_genre';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieGenreService extends CrudService<MovieGenre> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/MovieGenre/movie/{movieId}`)
    }
}
