import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentService } from '../../student/student.service';
import { StudentModel } from '../../student/student.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private studentService = inject(StudentService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  student: StudentModel | null = null;
  isEditingEmail = signal<boolean>(false);
  isEditingPassword = signal<boolean>(false);
  emailForm!: FormGroup;
  passwordForm!: FormGroup;
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.fetchProfile();
    this.initForms();
  }

  fetchProfile(): void {
    this.studentService.fetchLoggedInStudent().subscribe({
      next: (response) => {
        this.student = response.palyload ?? null;
      },
      error: () => {
        this.errorMessage.set('Błąd pobierania profilu użytkownika');
      }
    });
  }

  initForms(): void {
    this.emailForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.passwordForm = this.formBuilder.group({
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  toggleEmailEdit(): void {
    this.isEditingEmail.set(!this.isEditingEmail());
    if (this.student) {
      this.emailForm.patchValue({ email: this.student.email });
    }
  }

  updateEmail(): void {
    if (this.emailForm.invalid || !this.student) return;
    
    const updatedStudent = { ...this.student, email: this.emailForm.value.email };
    this.studentService.updateStudent(updatedStudent).subscribe({
      next: () => {
        this.successMessage.set('Email został zaktualizowany.');
        this.isEditingEmail.set(false);
        this.fetchProfile();
      },
      error: () => {
        this.errorMessage.set('Błąd podczas aktualizacji emaila.');
      }
    });
  }

  togglePasswordEdit(): void {
    this.isEditingPassword.set(!this.isEditingPassword());
  }

  updatePassword(): void {
    if (this.passwordForm.invalid) return;
    
    this.studentService.resetPassword('dummy-token', this.passwordForm.value.newPassword).subscribe({
      next: () => {
        this.successMessage.set('Hasło zostało zmienione.');
        this.isEditingPassword.set(false);
      },
      error: () => {
        this.errorMessage.set('Błąd podczas zmiany hasła.');
      }
    });
  }

  logout(): void {
    this.studentService.logout(); 
    this.router.navigate(['/login']);
  }
}

