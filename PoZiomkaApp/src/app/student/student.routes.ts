import { Routes } from "@angular/router";
import { ProfileComponent } from "./profile/profile.component";
import { ApplicationsComponent } from "./applications/applications.component";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'profile',
    title: 'PoZiomka - Student',
    pathMatch: 'full'
  },
  {
    path: 'profile',
    title: 'PoZiomka - Profil Studenta',
    component: ProfileComponent
  },
  {
    path: 'applications',
    title: 'PoZiomka - Wnioski',
    component: ApplicationsComponent
  }
]