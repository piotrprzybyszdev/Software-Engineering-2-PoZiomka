import { inject, Injectable } from "@angular/core";
import { ApiResponse, pipeApiResponse } from "../../common/api";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { RoomCreate, RoomModel } from "./room.model";

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private httpClient = inject(HttpClient);

  getRooms(): Observable<ApiResponse<RoomModel[]>> {
    return pipeApiResponse(this.httpClient.get<RoomModel[]>('/api/room/get'));
  }

  getRoom(id: number): Observable<ApiResponse<RoomModel>> {
    return pipeApiResponse(this.httpClient.get<RoomModel>(`/api/room/get/${id}`));
  }

  createRoom(roomCreate: RoomCreate): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.post<void>('/api/room/create', roomCreate));
  }

  addStudentToRoom(id: number, studentId: number): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/room/add-student', {
      id: id,
      studentId: studentId
    }));
  }

  removeStudentFromRoom(id: number, studentId: number): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.put<void>('/api/room/remove-student', {
      id: id,
      studentId: studentId
    }));
  }

  deleteRoom(id: number): Observable<ApiResponse<void>> {
    return pipeApiResponse(this.httpClient.delete<void>(`/api/room/delete/${id}`));
  }
}