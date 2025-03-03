import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { environment } from "../../environments/environment";
import { IListagemDeclaracaoEntregues } from "../interfaces/IListagemDeclaracaoEntregues";

@Injectable({
  providedIn: 'root'
})
export class DeclaracoesService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Fetch declarations by user ID
  getDeclaracoesByUserId(userId: string): Observable<IListagemDeclaracaoEntregues[]> {
    return this.http.get<IListagemDeclaracaoEntregues[]>(`${this.apiUrl}/declaracoes-iva/${userId}`)
      .pipe(
        catchError(error => {
          console.error('Error fetching declarações IVA:', error);
          return throwError(() => new Error('Failed to fetch declarações IVA'));
        })
      );
  }
}
