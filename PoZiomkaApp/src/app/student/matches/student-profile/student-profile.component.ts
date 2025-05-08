import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { PopupComponent } from "../../../common/popup/popup.component";
import { StudentService } from '../../student.service';
import { ToastrService } from 'ngx-toastr';
import { StudentModel } from '../../student.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-student-profile',
  imports: [PopupComponent, CommonModule],
  templateUrl: './student-profile.component.html',
  styleUrl: './student-profile.component.css'
})
export class StudentProfileComponent implements OnInit {
  studentId = input.required<number>();

  hide = output<void>();

  private studentService = inject(StudentService);
  private toastrService = inject(ToastrService);

  student = signal<StudentModel | undefined>(undefined);

  ngOnInit(): void {
    this.studentService.getStudent(this.studentId()).subscribe({
      next: response => {
        if (response.success) {
          this.student.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    })
  }

  onHide(): void {
    this.hide.emit();
  }
}
