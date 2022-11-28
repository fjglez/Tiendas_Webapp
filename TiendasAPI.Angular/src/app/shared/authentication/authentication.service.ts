import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILoggingUser } from './ILoggingUser';
import { Observable, tap, catchError, throwError, map } from 'rxjs';
import { ILoggingResult } from './iloggingresult';
import { IRegisterForm } from './iregisterform';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl = "https://localhost:7202/api"

  public token = "";
  public expiration = Date.now();
  public username = "";

  constructor(private http: HttpClient) {
  }

  login(user: ILoggingUser) {
    console.log("Iniciando sesión");
    return this.http.post<ILoggingResult>(this.baseUrl + "/authentication", user).pipe(
        tap(_ => console.log('Iniciada sesión con éxito.')),
      map(data => {
          this.username = data.username;
          this.token = data.token;
          this.expiration = Date.parse(data.expiration);
        }),
        catchError(this.handleError)
    );
  }

  register(user: IRegisterForm) {
    console.log("Creando nuevo usuario");
    return this.http.post(this.baseUrl + "/authentication/register", user).pipe(
      tap(_ => console.log('Registrado con éxito.')),
      catchError(this.handleError)
    );

  }

  logout() {
    this.username = "";
    this.token = "";
    this.expiration = Date.now();
  }

  isLogged(): boolean {
    return this.token.length > 0 && this.expiration >= Date.now();
  }

  private handleError(err: HttpErrorResponse) {
    return throwError(() => err.error)
  }
}
