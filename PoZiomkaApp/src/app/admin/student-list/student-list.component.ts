import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { StudentService } from '../../student/student.service';  
import { StudentCreate, StudentModel } from '../../student/student.model'; 
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-students-list',
  imports: [],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentsListComponent implements OnInit {
  private toastrService = inject(ToastrService);

  students: StudentModel[] = [];

  isStudentEditMode: WritableSignal<boolean>[] = [];
  isAddingStudent = signal<boolean>(false);

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
          this.isStudentEditMode = Array(this.students.length).fill(signal<boolean>(false));
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onStudentEdit(studentIndex: number): void {
    this.isStudentEditMode[studentIndex].set(true);
  }

  onStudentSave(studentIndex: number): void {
    this.isStudentEditMode[studentIndex].set(false);
    this.studentService.updateStudent(this.students[studentIndex]).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Dane studenta zostały pomyślnie zapisane');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.fetchStudents();
      }
    });
  }

  onStudentDelete(studentIndex: number): void {
    this.studentService.deleteStudent(this.students[studentIndex].id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Student został pomyślnie usunięty z systemu');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.fetchStudents();
      }
    });
  }

  onStudentAdd(): void {
    this.isAddingStudent.set(true);
  }

  onStudentRegister(): void {
    this.studentService.createStudent(this.studentCreate()).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Student został pomyślnie dodany do systemu');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.fetchStudents();
      }
    });
    this.isAddingStudent.set(false);
  }
}
