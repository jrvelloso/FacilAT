import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface User {
  Id: string;
  Name: string;
  Email: string;
  Password: string;
  NIF: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl; // Centralized API URL

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/users/`);
  }

  createUser(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/users/`, user);
  }

  updateUser(user: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/users/${user.id}`, {
      name: user.name,
      email: user.email,
      nif: user.nif,
      password: user.password
    });
  }

  logUserAction(userId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/user-actions/`, { user_id: userId });
  }
}
