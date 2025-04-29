import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuComponent } from "../common/menu/menu.component";
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-admin',
  imports: [RouterModule, MenuComponent, MenuComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  onApplicationListClick(): void {
    this.router.navigate(['/admin/applications']);
  }

  onDashboardClick(): void {
    this.router.navigate(['/admin/dashboard']);
  }

  onStudentListClick(): void {
    this.router.navigate(['/admin/students']);
  }

  onRoomListClick(): void {
    this.router.navigate(['/admin/rooms']);
  }

  onFormListClick(): void {
    this.router.navigate(['/admin/forms']);
  }

  onReservationsClick(): void {
    this.router.navigate(['/admin/reservations']);
  }

  onCommunicationsClick(): void {
    this.router.navigate(['/admin/communications']);
  }

  logout(): void {
    this.authService.logOut().subscribe({
      next: _ => this.router.navigate(['/login'])
    });
  }
}
