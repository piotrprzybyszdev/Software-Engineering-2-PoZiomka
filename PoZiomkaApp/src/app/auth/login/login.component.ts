import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { StudentService } from '../../student/student.service';
import { CardConfiguration, CenteredCardComponent } from '../../common/centered-card/centered-card.component';
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, CenteredCardComponent, LoadingButtonComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private studentService = inject(StudentService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  CardConfiguration = CardConfiguration;
  
  isLoading = signal<boolean>(false);
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

    this.isLoading.set(true);
    this.authService.logIn(this.loginForm.value.email!, this.loginForm.value.password!)
      .subscribe({
        next: response => { 
          if (response.success) {
            this.studentService.fetchLoggedInStudent().subscribe({
              next: response => {
                if (response.success) {
                  this.router.navigate(['/student/profile']); 
                } else {
                  this.isLoading.set(false);
                  this.error.set(response.error!.detail);
                }
              }
            })
          } else {
            this.isLoading.set(false);
            this.error.set(response.error!.detail);
          }
        }
    });
  }
}
