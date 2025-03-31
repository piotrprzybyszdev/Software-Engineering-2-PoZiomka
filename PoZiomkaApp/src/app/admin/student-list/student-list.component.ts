import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { StudentService } from '../../student/student.service';  
import { StudentCreate, StudentModel } from '../../student/student.model'; 

@Component({
  selector: 'app-students-list',
  imports: [],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentsListComponent implements OnInit {
  students: StudentModel[] = [];
  errorMessage: string | null = null;

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
          this.errorMessage = 'Nie udało się załadować studentów';
        }
      },
      error: (err) => {
        console.error('Błąd pobierania studentów:', err);
        this.errorMessage = 'Wystąpił błąd podczas pobierania danych';
      }
    });
  }

  onStudentEdit(studentIndex: number): void {
    this.isStudentEditMode[studentIndex].set(true);
  }

  onStudentSave(studentIndex: number): void {
    this.isStudentEditMode[studentIndex].set(false);
    this.studentService.updateStudent(this.students[studentIndex]).subscribe({
      next: _ => {
        this.fetchStudents();
      }
    });
  }

  onStudentDelete(studentIndex: number): void {
    this.studentService.deleteStudent(this.students[studentIndex].id).subscribe({
      next: _ => {
        this.fetchStudents();
      }
    });
  }

  onStudentAdd(): void {
    this.isAddingStudent.set(true);
  }

  onStudentRegister(): void {
    this.studentService.createStudent(this.studentCreate()).subscribe({
      next: _ => {
        this.fetchStudents();
      }
    });
    this.isAddingStudent.set(false);
  }
}
