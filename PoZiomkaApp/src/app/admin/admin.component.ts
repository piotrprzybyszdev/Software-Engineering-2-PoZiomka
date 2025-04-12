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

  onProfileClick(): void {
    this.router.navigate(['/admin/profile']);
  }

  onStudentListClick(): void {
    this.router.navigate(['/admin/students']);
  }

  onRoomListClick(): void {
    this.router.navigate(['/admin/rooms']);
  }

  logout(): void {
    this.authService.logOut().subscribe({
      next: _ => this.router.navigate(['/login'])
    });
  }
}
