import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { AdminProfileComponent } from "./profile/profile.component";
import { StudentsListComponent } from "./student-list/student-list.component";
import { RoomListComponent } from "./room/room-list/room-list.component";

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
    title: 'PoZiomka - Lista Studentów',
    component: StudentsListComponent
  },
  {
    path: 'rooms',
    title: 'PoZiomka - Lista Pokoi',
    component: RoomListComponent
  }
]