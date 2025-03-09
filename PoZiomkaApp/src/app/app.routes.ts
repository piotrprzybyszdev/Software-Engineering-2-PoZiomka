import { Routes } from "@angular/router";
import { LoginComponent } from "./student/auth/login/login.component";
import { SignupComponent } from "./student/auth/signup/signup.component";
import { NotFoundComponent } from "./not-found/not-found.component";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    title: 'Poziomka',
    pathMatch: 'full'
  },
  {
    path: 'login',
    title: 'Poziomka - Zaloguj się',
    component: LoginComponent
  },
  {
    path: 'signup',
    title: 'Poziomka - Zarejestruj się',
    component: SignupComponent
  },
  {
    path: '**',
    title: 'Poziomka - Nie znaleziono',
    component: NotFoundComponent
  }
]