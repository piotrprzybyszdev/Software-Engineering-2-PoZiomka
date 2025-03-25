import { Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { AdminLoginComponent } from "./login/admin-login.component";
import { SignupComponent } from "./signup/signup.component";
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";

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
    path: 'admin/login',
    title: 'Poziomka - Zaloguj się',
    component: AdminLoginComponent
  },
  {
    path: 'signup',
    title: 'Poziomka - Zarejestruj się',
    component: SignupComponent
  },
  {
    path: 'unauthorized/:role',
    title: 'Poziomka - Brak Uprawnień',
    component: UnauthorizedComponent
  }
]