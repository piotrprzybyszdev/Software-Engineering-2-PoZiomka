import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { Observable, tap } from "rxjs";
import { StudentModel } from "./student.model";
import { ApiResponse, pipeApiResponse } from "../../common/api";

@Injectable({
  providedIn: 'root'
})
export class StudentAuthService {
  private httpClient = inject(HttpClient);

  private _loggedInStudent = signal<StudentModel | null>(null);

  loggedInStudent = this._loggedInStudent.asReadonly();

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

  logOut(): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/logout', {}));
  }

  fetchLoggedInStudent(): Observable<ApiResponse<StudentModel | null>> {
    return pipeApiResponse(this.httpClient.get<StudentModel | null>('/api/student/get-logged-in').pipe(tap({
      next: student => this._loggedInStudent.set(student)
    })));
  }
}