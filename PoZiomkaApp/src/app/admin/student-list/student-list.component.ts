import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { StudentService } from '../../student/student.service';  
import { StudentCreate, StudentModel } from '../../student/student.model'; 
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-students-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentsListComponent implements OnInit {
  private toastrService = inject(ToastrService);

  students: StudentModel[] = [];

  editedStudentIndex = signal<number | null>(null); 
  isAddingStudent = signal<boolean>(false);

  studentCreate = signal<StudentCreate>({ email: "" });

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.fetchStudents();
  }

  fetchStudents(): void {
    this.studentService.getAllStudents().subscribe({
      next: (response) => {
        if (response.success) {
          this.students = response.palyload ?? [];
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onStudentEdit(studentIndex: number): void {
    this.editedStudentIndex.set(studentIndex); 
  }

  onStudentSave(studentIndex: number): void {
    const student = this.students[studentIndex];
    this.studentService.updateStudent({
      id: student.id,
      firstName: student.firstName,
      lastName: student.lastName,
      phoneNumber: student.phoneNumber,
      indexNumber: student.indexNumber
    }).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Dane studenta zostały pomyślnie zapisane');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.fetchStudents();
      }
    });

    this.editedStudentIndex.set(null);
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
    this.studentCreate.set({ email: "" });
    this.isAddingStudent.set(true);
    this.editedStudentIndex.set(null); 
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
