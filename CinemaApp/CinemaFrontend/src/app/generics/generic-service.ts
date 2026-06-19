import { Injectable, InjectionToken, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
// import { } from '../environments/environment';

// Define an InjectionToken for the base URL
export const BASE_URL = new InjectionToken<string>('BASE_URL');


@Injectable({
    providedIn: 'root'
})
export class CrudService<T> {

    constructor(
        private httpClient: HttpClient,
        @Inject(BASE_URL) protected baseUrl: string
    ) { }

    getAll(): Observable<T[]> {
        return this.httpClient.get<T[]>(`${this.baseUrl}`);
    }

    getOne(id: number): Observable<T> {
        return this.httpClient.get<T>(`${this.baseUrl}/${id}`);
    }

    create(item: T): Observable<T> {
        return this.httpClient.post<T>(`${this.baseUrl}`, item);
    }

    update(id: number, item: T): Observable<T> {
        return this.httpClient.put<T>(`${this.baseUrl}/${id}`, item);
    }

    delete(id: number): Observable<void> {
        return this.httpClient.delete<void>(`${this.baseUrl}/${id}`);
    }
}