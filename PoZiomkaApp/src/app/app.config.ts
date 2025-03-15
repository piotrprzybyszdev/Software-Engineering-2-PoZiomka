import { provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import { ApplicationConfig, LOCALE_ID, provideZoneChangeDetection } from "@angular/core";
import { provideRouter, withComponentInputBinding } from "@angular/router";
import { routingInterceptorProvider } from "./routing-interceptor";
import { routes } from "./app.routes";
import { registerLocaleData } from "@angular/common";
import localePL from "@angular/common/locales/pl";

registerLocaleData(localePL);

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptorsFromDi()),
    routingInterceptorProvider,
    { provide: LOCALE_ID, useValue: 'pl-PL' }
  ]
}