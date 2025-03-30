import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentService } from '../../student/student.service';
import { StudentModel } from '../../student/student.model';

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

  student: StudentModel | null = null;
  isEditingData = signal<boolean>(false);
  isEditingPassword = signal<boolean>(false);
  dataForm!: FormGroup;
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
        if (this.student) {
          this.dataForm.patchValue(this.student);
        }
      },
      error: () => {
        this.errorMessage.set('Błąd pobierania profilu użytkownika');
      }
    });
  }

  initForms(): void {
    this.dataForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      indexNumber: [''],
      phoneNumber: ['']
    });

    this.passwordForm = this.formBuilder.group({
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  toggleDataEdit(): void {
    this.isEditingData.set(!this.isEditingData());
    if (this.student) {
      this.dataForm.patchValue(this.student);
    }
  }

  updateData(): void {
    console.log("Próba aktualizacji danych...");

    if (this.dataForm.invalid || !this.student) {
        console.log("❌ Formularz niepoprawny lub brak studenta!");
        return;
    }
    
    const updatedStudent = { ...this.student, ...this.dataForm.value };
    console.log('Próba aktualizacji:', updatedStudent);
    this.studentService.updateStudent(updatedStudent).subscribe({
      next: () => {
        console.log('Aktualizacja zakończona sukcesem');
        this.successMessage.set('Dane zostały zaktualizowane.');
        this.isEditingData.set(false);
        this.fetchProfile();
      },
      error: (err) => {
        console.error('Błąd aktualizacji:', err);
        this.errorMessage.set('Błąd podczas aktualizacji danych.');
      }
    });
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
}


