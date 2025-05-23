import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StudentService } from '../../student/student.service';
import { CardConfiguration, CenteredCardComponent } from "../../common/centered-card/centered-card.component";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, CenteredCardComponent],
  templateUrl: './password-reset.component.html',
  styleUrl: './password-reset.component.css'
})
export class ResetPasswordComponent {
  private studentService = inject(StudentService);
  private toastrService = inject(ToastrService);
  private formBuilder = inject(FormBuilder);

  CardConfiguration = CardConfiguration;

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
        next: response => {
          if (response.success) {
            this.toastrService.success('Link do resetowanie hasła został wysłany')
          } else {
            this.toastrService.error(response.error!.detail, response.error!.title);
          }
        }
      });
  }
}
