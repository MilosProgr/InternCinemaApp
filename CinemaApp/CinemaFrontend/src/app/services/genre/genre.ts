import { Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { Genre } from '../../models/genre.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Service()
export class GenreService extends CrudService<Genre>{
    constructor(private http: HttpClient) {
        super(http, `${environment.baseUrl}/api/Genre`);
    } 
}
