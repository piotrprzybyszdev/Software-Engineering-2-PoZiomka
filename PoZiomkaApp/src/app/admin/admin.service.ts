import { inject, Injectable, signal } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../common/api";
import { Observable, tap } from "rxjs";
import { AdminModel } from "./admin.model";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private httpClient = inject(HttpClient);
  private _loggedInAdmin = signal<AdminModel | null>(null);

  loggedInAdmin = this._loggedInAdmin.asReadonly();

  fetchLoggedInAdmin(): Observable<ApiResponse<AdminModel | null>> {
    return pipeApiResponse(this.httpClient.get<AdminModel | null>('/api/admin/get-logged-in').pipe(tap({
      next: admin => this._loggedInAdmin.set(admin)
    })));
  }
}