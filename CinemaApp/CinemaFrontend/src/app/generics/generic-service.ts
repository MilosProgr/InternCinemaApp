import { Injectable, InjectionToken, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Link, PagedResult } from '../models/hateoas.model';
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

    getPaged(page: number = 1, pageSize: number = 10): Observable<PagedResult<T>> {
    return this.httpClient.get<PagedResult<T>>(
        `${this.baseUrl}?page=${page}&pageSize=${pageSize}`
    );
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

     // Izvršava bilo koji Link
    executeLink<R = any>(link: Link, body?: any): Observable<R> {
        const method = link.method.toUpperCase();
        switch (method) {
            case 'GET':    return this.httpClient.get<R>(link.href);
            case 'POST':   return this.httpClient.post<R>(link.href, body);
            case 'PUT':    return this.httpClient.put<R>(link.href, body);
            case 'PATCH':  return this.httpClient.patch<R>(link.href, body);
            case 'DELETE': return this.httpClient.delete<R>(link.href);
            default: throw new Error(`Nepoznat HTTP metod: ${method}`);
        }
    }

    // Helper — pronađi link po rel-u
    findLink(links: Link[], rel: string): Link | undefined {
        return links.find(l => l.rel === rel);
    }
}