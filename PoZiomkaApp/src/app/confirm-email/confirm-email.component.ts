import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { StudentService } from '../student/student.service';

@Component({
  selector: 'app-confirm-email',
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent {
  isSuccess: boolean = false;
  isError: boolean = false;

  private router = inject(Router);
  private studentService = inject(StudentService);
  token = input.required<string>();

  confirmEmail(): void {
    this.studentService.confirmStudent(this.token()).subscribe({
      next: response => {
        if (response.success) {
          this.router.navigate(['/login']);
        } else {
          this.isError = true;
        }
      }
    });
  }
}