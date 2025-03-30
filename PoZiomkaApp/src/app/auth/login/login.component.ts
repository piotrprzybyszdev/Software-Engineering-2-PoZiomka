import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { StudentService } from '../../student/student.service'; // Importujemy StudentService
import { CardConfiguration, CenteredCardComponent } from '../../common/centered-card/centered-card.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, CenteredCardComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private studentService = inject(StudentService); // Inicjalizujemy StudentService
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  CardConfiguration = CardConfiguration;
  
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

    this.authService.logIn(this.loginForm.value.email!, this.loginForm.value.password!)
      .subscribe({
        next: response => {
          if (response.success) {
            console.log("udane logowanie!");
            //this.router.navigate(['/student/profile']);
            // Po udanym logowaniu, pobieramy dane zalogowanego studenta
            this.studentService.fetchLoggedInStudent().subscribe({
              next: studentResponse => {
                if (studentResponse.success && studentResponse.palyload) {
                  // Przekierowanie na stronę profilu studenta
                  this.router.navigate(['/student/profile']);
                } else {
                  this.error.set('Nie udało się załadować profilu studenta.');
                }
              },
              error: () => {
                this.error.set('Błąd podczas pobierania danych studenta.');
              }
            });
          } else {
            if (response.error?.detail === "Exception of type 'PoZiomkaDomain.Student.EmailNotFoundException' was thrown.") {
              this.error.set('Student o tym emailu nie istnieje');
            } else {
              this.error.set(response.error?.detail);
            }
          }
        },
        error: () => {
          this.error.set('Wystąpił błąd podczas logowania.');
        }
      });
  }

  
  
  
}

