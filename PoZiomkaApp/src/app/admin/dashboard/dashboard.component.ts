import { Component, computed, inject } from '@angular/core';
import { AdminService } from '../admin.service';
import { Tile, TileListComponent } from "../../common/tile-list/tile-list.component";

@Component({
  selector: 'app-dashboard',
  imports: [TileListComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  private adminService = inject(AdminService);

  admin = computed(() => this.adminService.loggedInAdmin()!);

  tiles: Tile[] = [{
    title: "Lista Studentów",
    description: "Tutaj możesz edytować dane studentów oraz rejestrować nowych studentów",
    icon: "person-circle",
    link: "/admin/students"
  }, {
    title: "Lista Pokojów",
    description: "Tutaj możesz przypisywać studentów do pokoi oraz tworzyć nowe pokoje",
    icon: "houses",
    link: "/admin/rooms"
  }, {
    title: "Zarządzanie Wnioskami",
    description: "Tutaj możesz podejmować decyzje na temat wniosków studentów",
    icon: "journals",
    link: "/admin/applications"
  }, {
    title: "Zarządzanie Ankietami",
    description: "Tutaj możesz tworzyć oraz edytować ankiety",
    icon: "card-checklist",
    link: "/admin/forms"
  }, {
    title: "Zarządzanie Rezerwacjami",
    description: "Tutaj możesz akceptować proponowane rezerwacje studentów",
    icon: "calendar",
    link: "/admin/reservations"
  }, {
    title: "Komunikaty",
    description: "Tutaj możesz wysyłać komuniakty do wybranych studentów",
    icon: "chat-left",
    link: "/admin/communications"
  }];
}
