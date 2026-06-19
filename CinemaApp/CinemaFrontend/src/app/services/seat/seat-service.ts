import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { Seat } from '../../models/seat.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SeatService extends CrudService<Seat> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/Seat`)
    }
}
