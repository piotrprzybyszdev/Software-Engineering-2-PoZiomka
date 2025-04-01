import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentService } from '../../student/student.service';
import { StudentModel } from '../../student/student.model';
import { ToastrService } from 'ngx-toastr';

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
  private toastrService = inject(ToastrService);

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
        if (response.success) {
          this.student = response.palyload!;
          if (this.student) {
            this.dataForm.patchValue(this.student);
          }
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
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
    if (this.dataForm.invalid || !this.student) {
      return;
    }
    
    const updatedStudent = { ...this.student, ...this.dataForm.value };
    this.studentService.updateStudent(updatedStudent).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Aktualizacja zakończona sukcesem');
          this.isEditingData.set(false);
          this.fetchProfile();
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.fetchProfile();
      }
    });
  }
}
