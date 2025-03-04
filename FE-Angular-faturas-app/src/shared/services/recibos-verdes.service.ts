import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { IListagemRecibosVerdes } from '../interfaces/IListagemRecibosVerdes';

@Injectable({
  providedIn: 'root'
})
export class RecibosVerdesService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Fetch recibos verdes by user ID
  getRecibosVerdesByUserId(userId: string): Observable<IListagemRecibosVerdes[]> {
    return this.http.get<IListagemRecibosVerdes[]>(`${this.apiUrl}/recibos-verdes/${userId}`)
      .pipe(
        catchError(error => {
          console.error('Error fetching recibos verdes:', error);
          return throwError(() => new Error('Failed to fetch recibos verdes'));
        })
      );
  }
}
