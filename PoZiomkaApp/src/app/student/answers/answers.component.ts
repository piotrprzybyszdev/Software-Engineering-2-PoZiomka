import { Component, inject, OnInit, signal } from '@angular/core';
import { FormService } from '../../admin/forms/form.service';
import { FormContentModel, FormModel } from '../../admin/forms/form.model';
import { ToastrService } from 'ngx-toastr';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';
import { AnswerModel, AnswerStatus, FormStatus } from './answer.model';
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
  selectedAnswer = signal<AnswerModel | undefined>(undefined);


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

  getAnswerStatusForForm(formId: number): AnswerStatus | undefined {
    return this.answers().find(ans => ans.form.id === formId);
  }
  
  getActionLabel(formId: number): string {
    const status = this.getAnswerStatusForForm(formId)?.status;
    switch (status) {
      case 0: // NotFilled
        return 'Wypełnij ankietę';
      case 1: // InProgress
        return 'Kontynuuj wypełnianie';
      case 2: // Filled
        return 'Pokaż odpowiedzi';
      default:
        return 'Wypełnij ankietę';
    }
  }

  getButtonClass(formId: number): string {
    const status = this.getAnswerStatusForForm(formId)?.status;
    switch (status) {
      case 0: // NotFilled
        return 'btn btn-primary me-2'; 
      case 1: // InProgress
        return 'btn btn-warning me-2'; 
      case 2: // Filled
        return 'btn btn-success me-2'; 
      default:
        return 'btn btn-secondary me-2'; 
    }
  }
  
  
  onHandleForm(form: FormModel): void {
    const status = this.getAnswerStatusForForm(form.id)?.status;
  
    if (status === undefined || status === 0) { 
      this.onShowForm(form);
    } else if (status === 1) { 
      const studentId = this.getStudentId();
      if (studentId !== undefined) {
        this.answerService.getAnswers(form.id, studentId).subscribe({
          next: (res) => {
            if (res.success) {
              this.formService.getFormContent(form.id).subscribe({
                next: (formContentRes) => {
                  if (formContentRes.success) {
                    this.selectedForm.set(formContentRes.payload);
                    this.selectedAnswer.set(res.payload); 
                  } else {
                    this.toastr.error('Nie udało się pobrać pełnej treści formularza');
                  }
                },
                error: () => {
                  this.toastr.error('Błąd pobierania treści formularza');
                }
              });
            } else {
              this.toastr.error('Nie udało się pobrać Twojej odpowiedzi');
            }
          }
        });
      }
    } else if (status === 2) { 
      this.onShowAnswers(form.id);
    }
  }
  
  

  onHideForm(): void {
    this.selectedForm.set(undefined);
    this.selectedAnswer.set(undefined);
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

