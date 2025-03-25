import { Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { AdminLoginComponent } from "../admin/admin-login/admin-login.component";
import { SignupComponent } from "./signup/signup.component";
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";
import { ResetPasswordComponent } from "./password-reset/password-reset.component";
import { ResetPasswordConfirmComponent } from "./password-reset-confirm/password-reset-confirm.component";


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
    path: 'password-reset',
    title: 'Poziomka - Resetowanie Hasła',
    component: ResetPasswordComponent
  },
  {
    path: 'password-reset/:token',
    title: 'Poziomka - Nowe Hasło',
    component: ResetPasswordConfirmComponent
  },
  {
    path: 'unauthorized/:role',
    title: 'Poziomka - Brak Uprawnień',
    component: UnauthorizedComponent
  }
]