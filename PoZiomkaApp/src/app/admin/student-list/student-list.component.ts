import { Component, inject, OnInit, signal } from '@angular/core';
import { StudentService } from '../../student/student.service';  
import { StudentCreate, StudentModel } from '../../student/student.model'; 
import { ToastrService } from 'ngx-toastr';
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";

@Component({
  selector: 'app-students-list',
  imports: [LoadingButtonComponent],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentsListComponent implements OnInit {
  private toastrService = inject(ToastrService);

  students: StudentModel[] = [];

  isStudentEditMode = signal<boolean[]>([]);
  isAddingStudent = signal<boolean>(false);
  isLoading = signal<boolean[]>([]);
  isLoadingRegister = signal<boolean>(false);

  studentCreate = signal<StudentCreate>({email: ""});

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.fetchStudents();
  }

  fetchStudents(): void {
    this.studentService.getAllStudents().subscribe({
      next: (response) => {
        if (response.success) {
          this.students = response.palyload ?? [];
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
    narr[index] = true;
    return narr;
  }

  onStudentEdit(studentIndex: number): void {
    this.isStudentEditMode.update(arr => this.updateArr(arr, studentIndex, true));
  }

  onStudentSave(studentIndex: number): void {
    this.isLoading.update(arr => this.updateArr(arr, studentIndex, true));
    this.studentService.updateStudent(this.students[studentIndex]).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Dane studenta zostały pomyślnie zapisane');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isLoading.update(arr => this.updateArr(arr, studentIndex, false));
        this.isStudentEditMode.update(arr => this.updateArr(arr, studentIndex, false));
        this.fetchStudents();
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
    this.isAddingStudent.set(true);
  }

  onStudentRegister(): void {
    this.isLoadingRegister.set(true);
    this.studentService.createStudent(this.studentCreate()).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Student został pomyślnie dodany do systemu');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isLoadingRegister.set(false);
        this.isAddingStudent.set(false);
        this.fetchStudents();
      }
    });
  }
}
