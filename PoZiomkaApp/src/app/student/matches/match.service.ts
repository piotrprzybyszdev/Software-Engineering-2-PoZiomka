import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../../common/api";
import { MatchModel } from "./match.model";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private httpClient = inject(HttpClient);

  getStudentMatches(): Observable<ApiResponse<MatchModel[]>> {
    return pipeApiResponse(this.httpClient.get<MatchModel[]>('/api/match/get-student'));
  }

  updateMatch(id: number, isAcceptation: boolean): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/match/update', {
      id: id,
      isAcceptation: isAcceptation
    }));
  }
}