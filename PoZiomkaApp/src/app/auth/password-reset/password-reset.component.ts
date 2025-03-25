import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StudentService } from '../../student/student.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './password-reset.component.html',
  styleUrl: './password-reset.component.css'
})
export class ResetPasswordComponent {
  private studentService = inject(StudentService);
  private formBuilder = inject(FormBuilder);

  isSubmitted = signal<boolean>(false);
  errorMessage = signal<string | undefined>(undefined);
  successMessage = signal<string | undefined>(undefined);

  resetForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]]
  });

  // ✅ Getter sprawdzający czy email jest niepoprawny
  get emailInvalid(): boolean {
    const control = this.resetForm.get('email');
    return this.isSubmitted() && !!control?.invalid;
  }

  onResetPassword(): void {
    if (this.resetForm.invalid) {
      this.isSubmitted.set(true);
      return;
    }

    this.studentService.requestPasswordReset(this.resetForm.value.email!)
      .subscribe({
        next: () => {
          this.successMessage.set('Link do resetowania hasła został wysłany.');
          this.errorMessage.set(undefined);
        },
        error: () => {
          this.errorMessage.set('Nie udało się wysłać linku resetującego.');
          this.successMessage.set(undefined);
        }
      });
  }
}
