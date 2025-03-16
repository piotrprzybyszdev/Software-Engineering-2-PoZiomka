import { Injectable, signal } from "@angular/core";
import { ApiResponse } from "../common/api";
import { Observable, of } from "rxjs";
import { AdminModel } from "./admin.model";

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private _loggedInAdmin = signal<AdminModel | null>(null);

  loggedInAdmin = this._loggedInAdmin.asReadonly();

  fetchLoggedInAdmin(): Observable<ApiResponse<AdminModel>> {
    return of({
      success: false
    })
  }
}