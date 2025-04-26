import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../../common/api";
import { FormContentModel, FormCreate, FormModel, FormUpdate } from "./form.model";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class FormService {
  private httpClient = inject(HttpClient);

  getForms(): Observable<ApiResponse<FormModel[]>> {
    return pipeApiResponse(this.httpClient.get<FormModel[]>('/api/form/get'));
  }

  getFormContent(id: number): Observable<ApiResponse<FormContentModel>> {
    return pipeApiResponse(this.httpClient.get<FormContentModel>(`/api/form/get-content/${id}`));
  }

  createForm(formCreate: FormCreate): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/form/create', formCreate));
  }

  updateForm(formUpdate: FormUpdate): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/form/update', formUpdate));
  }

  deleteForm(id: number): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.delete<void>(`/api/form/delete/${id}`));
  }
}