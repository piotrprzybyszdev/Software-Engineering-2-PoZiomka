import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../common/api";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ApplicationModel, ApplicationSearchParams, ApplicationStatus, ApplicationTypeModel } from "./application.model";

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private httpClient = inject(HttpClient);

  getApplicationTypes(): Observable<ApiResponse<ApplicationTypeModel[]>> {
    return pipeApiResponse(this.httpClient.get<ApplicationTypeModel[]>('/api/aplication/get-types'));
  }

  submitApplication(typeId: number, file: File): Observable<ApiResponse<void>> {
    const formData = new FormData();
    formData.append("id", typeId.toString());
    formData.append("file", file);

    return pipeApiResponse(this.httpClient.post<void>('/api/aplication/get-types', formData));
  }

  resolveApplication(id: number, status: ApplicationStatus) {
    return pipeApiResponse(this.httpClient.put<void>('/api/application/resolve', {
        id: id,
        status: status
    }));
  }

  getApplications(params: ApplicationSearchParams): Observable<ApiResponse<ApplicationModel[]>> {
    return pipeApiResponse(this.httpClient.get<ApplicationModel[]>(
        `/api/aplication/get-types${encodeURIComponent(JSON.stringify(params))}`
    ));
  }
}