import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { AdminProfileComponent } from "./profile/profile.component";
import { StudentsListComponent } from "./students/students.component";
import { RoomListComponent } from "./rooms/rooms.component";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    title: 'PoZiomka - Administrator',
    pathMatch: 'full'
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
  },
  {
    path: 'students',
    title: 'PoZiomka - Studenci',
    component: StudentsListComponent
  },
  {
    path: 'rooms',
    title: 'PoZiomka - Pokoje',
    component: RoomListComponent
  }
]