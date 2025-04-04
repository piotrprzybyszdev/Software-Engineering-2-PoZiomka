import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentService } from '../../student/student.service';
import { StudentModel } from '../../student/student.model';
import { ToastrService } from 'ngx-toastr';
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LoadingButtonComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private studentService = inject(StudentService);
  private formBuilder = inject(FormBuilder);
  private toastrService = inject(ToastrService);
  private router = inject(Router);

  isLoading = signal<boolean>(false);
  student: StudentModel | null = null;
  isEditingData = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);
  dataForm!: FormGroup;

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
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      indexNumber: [''],
      phoneNumber: [''],
      isPhoneNumberHidden: [false],
      isIndexNumberHidden: [false]
    });
  }

  toggleDataEdit(): void {
    this.isEditingData.set(!this.isEditingData());
    if (this.student) {
      this.dataForm.patchValue(this.student);
    }
  }

  deleteAccount(): void {
    this.isLoading.set(true);
    this.studentService.deleteStudent(this.student!.id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie usunięto konto z systemu');
          this.isLoading.set(false);
          this.router.navigate(['/']);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.detail);
          this.isLoading.set(false);
        }
      }
    });
  }

  updateData(): void {
    if (this.dataForm.invalid || !this.student) {
      return;
    }

    const updatedStudent = { ...this.student, ...this.dataForm.getRawValue() };

    this.isLoading.set(true);
    this.studentService.updateStudent(updatedStudent).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Aktualizacja zakończona sukcesem');
          this.isEditingData.set(false);
          this.isLoading.set(false);
          this.fetchProfile();
        } else {
          this.isLoading.set(false);
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
}
