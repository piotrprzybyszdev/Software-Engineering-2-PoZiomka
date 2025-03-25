import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.css'
})
export class AdminLoginComponent {
  private authService = inject(AuthService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  
  isSubmitted = signal<boolean>(false);
  error = signal<string | undefined>(undefined);

  loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
    password: ['', [Validators.required, Validators.maxLength(100)]]
  });

  get emailInvalid(): boolean {
    const email = this.loginForm.controls.email;
    return email.invalid && (email.dirty || email.touched || this.isSubmitted());
  }

  get passwordInvalid(): boolean {
    const password = this.loginForm.controls.password;
    return password.invalid && (password.dirty || password.touched || this.isSubmitted());
  }

  onLoginButtonClick(): void {
    if (this.loginForm.invalid) {
      this.isSubmitted.set(true);
      return;
    }

    this.authService.adminLogIn(this.loginForm.value.email!, this.loginForm.value.password!)
      .subscribe({
        next: response => { 
          if (response.success) {
            this.router.navigate(['/admin/dashboard']); 
          } else {
            this.router.navigate(['/admin/dashboard']); // no ver yet
            //this.error.set(response.error?.detail);
          }
        }
    });
  }
}