import { inject, Injectable, signal } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../common/api";
import { StudentCreate, StudentModel, StudentUpdate } from "./student.model";
import { Observable, tap } from "rxjs";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private httpClient = inject(HttpClient);
  private authService = inject(AuthService);
  private router = inject(Router);
  private _loggedInStudent = signal<StudentModel | null>(null);

  loggedInStudent = this._loggedInStudent.asReadonly();

  fetchLoggedInStudent(): Observable<ApiResponse<StudentModel | null>> {
    return pipeApiResponse(this.httpClient.get<StudentModel | null>('/api/student/get-logged-in').pipe(tap({
      next: student => this._loggedInStudent.set(student)
    })));
  }

  confirmStudent(token: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/student/confirm', {
      token: token
    }));
  }

  requestPasswordReset(email: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/student/request-password-reset', {
      email: email
    }));
  }

  resetPassword(token: string, p0: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/student/password-reset/', {
      token: token
    }));
  }

  getAllStudents(): Observable<ApiResponse<StudentModel[]>> {
    return pipeApiResponse(this.httpClient.get<StudentModel[]>('/api/student/get'));
  }

  getStudent(id: number): Observable<ApiResponse<StudentModel>> {
    return pipeApiResponse(this.httpClient.get<StudentModel>(`/api/student/get/${id}`));
  }

  createStudent(studentCreate: StudentCreate) {
    return pipeApiResponse(this.httpClient.post<void>('/api/student/create', studentCreate));
  }

  updateStudent(studentUpdate: StudentUpdate) {
    return pipeApiResponse(this.httpClient.put<void>('/api/student/update', studentUpdate));
  }

  deleteStudent(id: number) {
    return pipeApiResponse(this.httpClient.delete<void>(`/api/student/delete/${id}`, {}));
  }
}