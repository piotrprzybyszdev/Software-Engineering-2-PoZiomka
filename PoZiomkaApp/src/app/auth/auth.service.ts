import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiResponse, pipeApiResponse } from "../common/api";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient);

  signUp(email: string, password: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/signup', {
      email: email,
      password: password
    }));
  }

  logIn(email: string, password: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(
      this.httpClient.post<void>('/api/login', {
        email: email,
        password: password
      })
    );
  }

  adminLogIn(email: string, password: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(
      this.httpClient.post<void>('/api/admin-login', {
        email: email,
        password: password
      })
    );
  }

  logOut(): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/logout', {}));
  }
}