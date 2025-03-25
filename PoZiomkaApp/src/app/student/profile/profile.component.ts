import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentService } from '../../student/student.service';
import { StudentModel } from '../../student/student.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private studentService = inject(StudentService);
  student: StudentModel | null = null;

  ngOnInit(): void {
    this.studentService.fetchLoggedInStudent().subscribe({
      next: (response) => {
        this.student = response.palyload ?? null;
      },
      error: () => {
        console.error('Błąd pobierania profilu użytkownika');
      }
    });
  }

  logout(): void {
    console.log("Wylogowano użytkownika");
  }
}

