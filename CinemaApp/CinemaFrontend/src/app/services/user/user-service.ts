import { Injectable, Service } from '@angular/core';
import { CrudService } from '../../generics/generic-service';
import { User } from '../../models/user.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService extends CrudService<User>{
    constructor(private http: HttpClient){
        super(http, `${environment.baseUrl}/User`)
    }
}
