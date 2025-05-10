import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuComponent } from "../common/menu/menu.component";
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-student',
  imports: [RouterModule, MenuComponent],
  templateUrl: './student.component.html',
  styleUrl: './student.component.css'
})
export class StudentComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  onMatchesClick(): void {
    this.router.navigate(['/student/matches']);
  }

  onFormsClick(): void {
    this.router.navigate(['/student/answers']);
  }

  onApplicationsClick(): void {
    this.router.navigate(['/student/applications']);
  }

  onProfileClick(): void {
    this.router.navigate(['/student/profile']);
  }

  logout(): void {
    this.authService.logOut().subscribe({
      next: _ => this.router.navigate(['/login'])
    });
  }
}
