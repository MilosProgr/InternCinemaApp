import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { ReservationSeat } from '../../models/reservation_seat.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReservationSeatService extends CrudService<ReservationSeat> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/ReservationSeat`)
    }
}
