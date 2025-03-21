import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentAuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  private authService = inject(StudentAuthService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  
  isSubmitted = signal<boolean>(false);
  error = signal<string | undefined>(undefined);

  signupForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
    password: ['', [Validators.required, Validators.maxLength(100)]]
  });

  get emailInvalid(): boolean {
    const email = this.signupForm.controls.email;
    return email.invalid && (email.dirty || email.touched || this.isSubmitted());
  }

  get passwordInvalid(): boolean {
    const password = this.signupForm.controls.password;
    return password.invalid && (password.dirty || password.touched || this.isSubmitted());
  }

  onRegisterButtonClick(): void {
    if (this.signupForm.invalid) {
      this.isSubmitted.set(true);
      return;
    }

    this.authService.signUp(this.signupForm.value.email!, this.signupForm.value.password!)
      .subscribe({
        next: response => { 
          if (response.success) {
            this.router.navigate(['/login']);
          } else {
            this.error.set(response.error?.detail);
          }
        }
    });
  }
}
