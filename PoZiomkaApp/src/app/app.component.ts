import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { StudentAuthService } from './student/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private router = inject(Router);
  private studentAuthService = inject(StudentAuthService);

  ngOnInit(): void {
    this.studentAuthService.fetchLoggedInStudent().subscribe({
      next: response => {
        if (response.palyload) {
          this.router.navigate(['/student/profile']);
        }
      }
    });
  }
}
