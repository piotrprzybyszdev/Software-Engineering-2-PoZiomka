import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse, toQueryString } from "../common/api";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ApplicationModel, ApplicationSearchParams, ApplicationStatus, ApplicationTypeModel } from "./application.model";

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private httpClient = inject(HttpClient);

  getApplicationTypes(): Observable<ApiResponse<ApplicationTypeModel[]>> {
    return pipeApiResponse(this.httpClient.get<ApplicationTypeModel[]>('/api/application/get-types'));
  }

  getApplications(params: ApplicationSearchParams): Observable<ApiResponse<ApplicationModel[]>> {
    return pipeApiResponse(this.httpClient.get<ApplicationModel[]>(
        `/api/application/get${toQueryString(params)}`
    ));
  }

  getStudentApplications(): Observable<ApiResponse<ApplicationModel[]>> {
    return pipeApiResponse(this.httpClient.get<ApplicationModel[]>('/api/application/get-student'));
  }

  submitApplication(id: number, file: File): Observable<ApiResponse<void>> {
    const formData = new FormData();
    formData.append("file", file);

    return pipeApiResponse(this.httpClient.post<void>(`/api/application/submit/${id}`, formData));
  }

  resolveApplication(id: number, status: ApplicationStatus): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>(`/api/application/resolve/${id}?status=${status}`, {}));
  }

  downloadApplicationFile(id: number): Observable<ApiResponse<Blob>> {
    return pipeApiResponse(this.httpClient.get<Blob>(`/api/application/download/${id}`, { responseType: 'blob' as 'json' }));
  }
}