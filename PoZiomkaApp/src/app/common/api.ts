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
  payload?: T
}

export function toQueryString(data: any): string {
  const keys = Object.keys(data);
  if (keys.length === 0) {
    return '';
  }

  return '?' + keys.map(key => `${encodeURIComponent(key)}=${encodeURIComponent(data[key])}`).join('&');
}

export function pipeApiResponse<T>(observable: Observable<T>): Observable<ApiResponse<T>> {
  return observable.pipe(map(response => {
    return {
      success: true,
      payload: response
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