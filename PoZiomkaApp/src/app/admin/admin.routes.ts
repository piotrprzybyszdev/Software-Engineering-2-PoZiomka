import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { StudentsListComponent } from "./students/students.component";
import { RoomListComponent } from "./rooms/rooms.component";
import { ApplicationListComponent } from "./applications/applications.component";
import { FormListComponent } from "./forms/forms.component";
import { FormEditComponent } from "./forms/form-edit/form-edit.component";
import { ReservationsComponent } from "./reservations/reservations.component";
import { CommunicationsComponent } from "./communications/communications.component";

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
    path: 'students',
    title: 'PoZiomka - Studenci',
    component: StudentsListComponent
  },
  {
    path: 'rooms',
    title: 'PoZiomka - Pokoje',
    component: RoomListComponent
  },
  {
    path: 'applications',
    title: 'PoZiomka - Wnioski',
    component: ApplicationListComponent
  },
  {
    path: 'forms',
    title: 'PoZiomka - Ankiety',
    component: FormListComponent
  },
  {
    path: 'reservations',
    title: 'PoZiomka - Rezerwacje',
    component: ReservationsComponent
  },
  {
    path: 'communications',
    title: 'PoZiomka - Komunikaty',
    component: CommunicationsComponent
  },
  {
    path: 'form/edit/:formId',
    title: 'PoZiomka - Ankieta',
    component: FormEditComponent
  }
]