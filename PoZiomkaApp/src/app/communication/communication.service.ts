import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../common/api";
import { Observable } from "rxjs";
import { CommunicationModel } from "./communication.model";

@Injectable({
  providedIn: 'root'
})
export class CommunicationService {
  private httpClient = inject(HttpClient);

  getStudentCommunication(): Observable<ApiResponse<CommunicationModel[]>> {
    return pipeApiResponse(this.httpClient.get<CommunicationModel[]>('/api/communication/get-student'));
  }

  sendCommunication(studentIds: number[], description: string): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/communication/send', {
      studentIds: studentIds,
      description: description
    }));
  }

  deleteCommunication(id: number): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.delete<void>(`/api/communication/delete/${id}`));
  }
}