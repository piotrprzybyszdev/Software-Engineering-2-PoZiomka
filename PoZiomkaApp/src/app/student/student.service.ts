import { Injectable, signal } from "@angular/core";
import { ApiResponse } from "../common/api";
import { StudentModel } from "./student.model";
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private _loggedInStudent = signal<StudentModel | null>(null);

  loggedInStudent = this._loggedInStudent.asReadonly();

  fetchLoggedInStudent(): Observable<ApiResponse<StudentModel>> {
    return of({
      success: false
    })
  }
}