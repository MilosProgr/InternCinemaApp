import { Injectable } from '@angular/core';import { environment } from '../../environments/environment';
import { CrudService } from '../../generics/generic-service';
import { Genre } from '../../models/genre.model';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class GenreService extends CrudService<Genre>{
    constructor(private http: HttpClient) {
        super(http, `${environment.baseUrl}/Genre`);
    } 
}
