import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../common/api";
import { Observable } from "rxjs";
import { ReservationModel, ReservationStudentModel } from "./reservation.model";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private httpClient = inject(HttpClient);

  getReservations(): Observable<ApiResponse<ReservationModel[]>> {
    return pipeApiResponse(this.httpClient.get<ReservationModel[]>('/api/reservation/get'));
  }

  getReservation(id: number): Observable<ApiResponse<ReservationStudentModel>> {
    return pipeApiResponse(this.httpClient.get<ReservationStudentModel>(`/api/reservation/get/${id}`));
  }

  updateReservation(id: number, isAcceptation: boolean): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/reservation/update', {
      id: id,
      isAcceptation: isAcceptation
    }));
  }
}
