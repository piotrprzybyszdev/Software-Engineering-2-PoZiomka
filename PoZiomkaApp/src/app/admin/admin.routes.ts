import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { AdminLoginComponent } from "./admin-login/admin-login.component";
import { AdminProfileComponent } from "./profile/profile.component";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    title: 'PoZiomka - Administrator',
    pathMatch: 'full'
  },
  {
    path: 'login',
    title: 'Poziomka - Zaloguj się',
    component: AdminLoginComponent
  },
  {
    path: 'dashboard',
    title: 'PoZiomka - Panel Administratora',
    component: DashboardComponent
  },
  {
    path: 'profile',
    title: 'PoZiomka - Profil Administratora',
    component: AdminProfileComponent 
  }
]