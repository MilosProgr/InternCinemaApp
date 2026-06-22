import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { ScreeningSeat } from '../../models/screeningSeat';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ScreeningSeatService extends CrudService<ScreeningSeat> {
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/screenings`)
    }
}
