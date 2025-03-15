import { HTTP_INTERCEPTORS, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Provider } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../environments/environment";

export class RoutingInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req.clone({
            url: environment.apiUrl.concat(req.url),
            withCredentials: true
        }));
    }
}

export const routingInterceptorProvider: Provider = {
    provide: HTTP_INTERCEPTORS, useClass: RoutingInterceptor, multi: true
};
