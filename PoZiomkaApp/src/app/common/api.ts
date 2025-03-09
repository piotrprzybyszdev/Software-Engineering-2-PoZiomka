import { HttpErrorResponse } from "@angular/common/http";
import { catchError, map, Observable, of, throwError } from "rxjs";

export type ApiError = {
  status: string,
  detail: string,
  title: string
}

export type ApiResponse<T> = {
  success: boolean,
  error?: ApiError,
  palyload?: T
}

export function pipeApiResponse<T>(observable: Observable<T>): Observable<ApiResponse<T>> {
  return observable.pipe(map(response => {
    return {
      success: true,
      palyload: response
    }
  }), catchError(error => {
    if (error instanceof HttpErrorResponse) {
      return of({
        success: false,
        error: error.error as ApiError
      });
    }

    return throwError(() => error);
  }));
}