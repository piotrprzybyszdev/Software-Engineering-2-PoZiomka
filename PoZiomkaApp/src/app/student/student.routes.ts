import { Routes } from "@angular/router";
import { ProfileComponent } from "./profile/profile.component";
import { StudentsListComponent } from '../11test/student-list.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'profile',
    title: 'PoZiomka - Student',
    pathMatch: 'full'
  },
  { path: 'students', component: StudentsListComponent },
  {
    path: 'profile',
    title: 'PoZiomka - Profil Studenta',
    component: ProfileComponent
  }
]