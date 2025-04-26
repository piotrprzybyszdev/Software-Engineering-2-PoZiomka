import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiResponse, pipeApiResponse } from "../../common/api";
import { AnswerCreate, AnswerModel, AnswerStatus, AnswerUpdate } from "./answer.model";

@Injectable({
  providedIn: 'root'
})
export class AnswerService {
  private httpClient = inject(HttpClient);

  getStudentAnswers(): Observable<ApiResponse<AnswerStatus[]>> {
    return pipeApiResponse(this.httpClient.get<AnswerStatus[]>('/api/answer/get-student'));
  }

  getAnswers(formId: number, studentId: number): Observable<ApiResponse<AnswerModel>> {
    return pipeApiResponse(this.httpClient.get<AnswerModel>(`/api/answer/get/${formId}/${studentId}`));
  }

  createAnswer(answerCreate: AnswerCreate): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/answer/create', answerCreate));
  }

  updateAnswer(answerUpdate: AnswerUpdate): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/answer/update', answerUpdate));
  }

  deleteAnswer(id: number): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.delete<void>(`/api/answer/delete/${id}`));
  }
}
