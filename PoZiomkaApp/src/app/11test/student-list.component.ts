import { Component, OnInit } from '@angular/core';
import { StudentService } from '../student/student.service';  
import { StudentModel } from '../student/student.model'; 

@Component({
  selector: 'app-students-list',
  imports: [],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentsListComponent implements OnInit {
  students: StudentModel[] = [];
  errorMessage: string | null = null;

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.studentService.getAllStudents().subscribe({
      next: (response) => {
        if (response.success) {
          this.students = response.palyload ?? []; 
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
}
