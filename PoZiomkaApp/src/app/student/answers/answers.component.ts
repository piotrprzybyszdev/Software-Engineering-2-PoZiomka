import { Component, inject, OnInit, signal } from '@angular/core';
import { FormService } from '../../admin/forms/form.service';
import { FormContentModel, FormModel } from '../../admin/forms/form.model';
import { ToastrService } from 'ngx-toastr';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';
import { AnswerModel, AnswerStatus } from './answer.model';
import { AnswerService } from './answer.service';
import { StudentService } from '../student.service';
import { AnswerViewComponent } from './answer-view/answer-view.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-answers',
  standalone: true,
  imports: [AnswerEditComponent, AnswerViewComponent, CommonModule],
  templateUrl: './answers.component.html',
  styleUrl: './answers.component.css'
})
export class AnswersComponent implements OnInit {
  private formService = inject(FormService);
  private answerService = inject(AnswerService);
  private studentService = inject(StudentService);
  private toastr = inject(ToastrService);

  forms = signal<FormModel[]>([]);
  answers = signal<AnswerStatus[]>([]);
  selectedForm = signal<FormContentModel | undefined>(undefined);
  selectedFormIdForView = signal<number | undefined>(undefined);
  studentId = signal<number | undefined>(undefined);

  ngOnInit(): void {
    this.loadForms();
    this.loadAnswers();
    this.loadLoggedInStudent();
  }

  loadForms(): void {
    this.formService.getForms().subscribe({
      next: (res) => {
        if (res.success) {
          this.forms.set(res.payload ?? []);
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd', res.error?.title ?? 'Błąd pobierania formularzy');
        }
      }
    });
  }

  loadAnswers(): void {
    this.answerService.getStudentAnswers().subscribe({
      next: (res) => {
        if (res.success) {
          this.answers.set(res.payload ?? []);
        }
      }
    });
  }

  loadLoggedInStudent(): void {
    this.studentService.fetchLoggedInStudent().subscribe({
      next: (res) => {
        if (res.success && res.payload) {
          this.studentId.set(res.payload.id);
        }
      }
    });
  }

  onShowForm(form: FormModel): void {
    this.formService.getFormContent(form.id).subscribe({
      next: (res) => {
        if (res.success) {
          this.selectedForm.set(res.payload!);
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd', res.error?.title ?? 'Błąd pobierania zawartości formularza');
        }
      }
    });
  }

  onHideForm(): void {
    this.selectedForm.set(undefined);
  }

  onShowAnswers(formId: number): void {
    this.selectedFormIdForView.set(formId);
  }

  onHideAnswers(): void {
    this.selectedFormIdForView.set(undefined);
  }

  hasAnswer(formId: number): boolean {
    return this.answers().some(answer => answer.form.id === formId);
  }

  getStudentId(): number | undefined {
    return this.studentId();
  }
}

