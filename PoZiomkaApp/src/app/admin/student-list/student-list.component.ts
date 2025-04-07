import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { StudentService } from '../../student/student.service';  
import { StudentCreate, StudentModel } from '../../student/student.model'; 
import { ToastrService } from 'ngx-toastr';
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-students-list',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingButtonComponent],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentsListComponent implements OnInit {
  private toastrService = inject(ToastrService);
  private studentService = inject(StudentService);

  uneditedStudents: StudentModel[] = [];
  students: StudentModel[] = [];

  isStudentEditMode = signal<boolean[]>([]);
  isAddingStudent = signal<boolean>(false);
  isLoading = signal<boolean[]>([]);
  isLoadingRegister = signal<boolean>(false);

  studentCreate = signal<StudentCreate>({ email: "" });
  isEditingStudent = computed(() => !this.isStudentEditMode().every(b => b === false));

  ngOnInit(): void {
    this.fetchStudents();
  }

  fetchStudents(): void {
    this.studentService.getAllStudents().subscribe({
      next: (response) => {
        if (response.success) {
          this.students = response.payload ?? [];
          this.uneditedStudents = structuredClone(this.students);
          this.isStudentEditMode.set(Array(this.students.length).fill(false));
          this.isLoading.set(Array(this.students.length).fill(false));
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  updateArr(arr: boolean[], index: number, value: boolean): boolean[] {
    const narr = [...arr];
    narr[index] = value;
    return narr;
  }

  onStudentEdit(studentIndex: number): void {
    this.isStudentEditMode.update(arr => this.updateArr(arr, studentIndex, true));
  }

  onStudentEditCancel(studentIndex: number): void {
    this.students[studentIndex] = structuredClone(this.uneditedStudents[studentIndex]);
    this.isStudentEditMode.update(arr => this.updateArr(arr, studentIndex, false));
  }

  onStudentSave(studentIndex: number): void {
    this.isLoading.update(arr => this.updateArr(arr, studentIndex, true));
    this.studentService.updateStudent(this.students[studentIndex]).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Dane studenta zostały pomyślnie zapisane');
          this.isStudentEditMode.update(arr => this.updateArr(arr, studentIndex, false));
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isLoading.update(arr => this.updateArr(arr, studentIndex, false));
      }
    });
  }
  
  onStudentDelete(studentIndex: number): void {
    this.isLoading.update(arr => this.updateArr(arr, studentIndex, true));
    this.studentService.deleteStudent(this.students[studentIndex].id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Student został pomyślnie usunięty z systemu');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isLoading.update(arr => this.updateArr(arr, studentIndex, false));
        this.fetchStudents();
      }
    });
  }

  onStudentAdd(): void {
    this.studentCreate.set({ email: "" });
    this.isAddingStudent.set(true);
  }

  onStudentAddCancel(): void {
    this.isAddingStudent.set(false);
  }

  onStudentRegister(): void {
    this.isLoadingRegister.set(true);
    this.studentService.createStudent(this.studentCreate()).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Student został pomyślnie dodany do systemu');
          this.isAddingStudent.set(false);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isLoadingRegister.set(false);
        this.fetchStudents();
      }
    });
  }
}
