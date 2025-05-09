import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { StudentService } from '../../student/student.service';
import { CardConfiguration, CenteredCardComponent } from "../../common/centered-card/centered-card.component";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password-confirm',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, CenteredCardComponent],
  templateUrl: './password-reset-confirm.component.html',
  styleUrl: './password-reset-confirm.component.css'
})
export class ResetPasswordConfirmComponent {
  private studentService = inject(StudentService);
  private formBuilder = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private toastrService = inject(ToastrService);

  CardConfiguration = CardConfiguration;

  token = this.route.snapshot.paramMap.get('token');
  isSubmitted = signal<boolean>(false);
  errorMessage = signal<string | undefined>(undefined);
  successMessage = signal<string | undefined>(undefined);

  resetForm = this.formBuilder.group({
    password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]]
  });

  onResetPassword(): void {
    if (this.resetForm.invalid) {
      this.isSubmitted.set(true);
      return;
    }

    this.studentService.resetPassword(this.token!, this.resetForm.value.password!)
      .subscribe({
        next: response => {
          if (response.success) {
            this.toastrService.success('Hasło zostało zmienione. Możesz się teraz zalogować.');
          } else {
            this.toastrService.error(response.error!.detail, response.error!.title);
          }
          this.router.navigate(['/login']);
        }
      });
  }
}
