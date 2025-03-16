import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { StudentService } from './student/student.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private studentService = inject(StudentService);
  private router = inject(Router);

  ngOnInit(): void {
    this.studentService.fetchLoggedInStudent().subscribe({
      next: response => {
        if (response.success) {
          this.router.navigate(['/student']);
        }
      }
    });
  }
}
