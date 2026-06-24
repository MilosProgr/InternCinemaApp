import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { CreateReservationRequest, Reservation } from '../../models/reservation.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReservationService extends CrudService<Reservation> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/Reservation`)
    }

    

    createReservation(
        request:CreateReservationRequest
    ){
        return this.http.post<Reservation>(
            `${this.baseUrl}`,
            request
        );
    }

    getMyReservations(): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(`${environment.baseUrl}/Reservation/my`);
    // headers sa tokenom se dodaju automatski preko interceptora
}

cancel(id: number): Observable<void> {
    return this.http.patch<void>(`${environment.baseUrl}/Reservation/${id}/cancel`, {});
}


}
