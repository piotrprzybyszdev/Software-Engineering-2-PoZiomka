import { Component, EventEmitter, Input, Output, OnInit, inject, signal } from '@angular/core';
import { AnswerService } from '../answer.service';
import { FormService } from '../../../admin/forms/form.service';
import { AnswerModel, AnswerUpdate } from '../answer.model';
import { FormContentModel } from '../../../admin/forms/form.model';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { AnswerEditComponent } from '../answer-edit/answer-edit.component';

@Component({
  selector: 'app-answer-view',
  standalone: true,
  imports: [PopupComponent, CommonModule, FormsModule, AnswerEditComponent],
  templateUrl: './answer-view.component.html',
  styleUrl: './answer-view.component.css'
})
export class AnswerViewComponent implements OnInit {
  @Input() formId!: number;
  @Input() studentId!: number;
  @Output() hide = new EventEmitter<void>();

  private answerService = inject(AnswerService);
  private formService = inject(FormService);
  private toastr = inject(ToastrService);

  choosableInput = '';
  choosableAnswers = signal<string[]>([]); 
  showEdit = false;

  formContent = signal<FormContentModel | null>(null);
  answer = signal<AnswerModel | undefined>(undefined);
  selectedAnswers = signal<Record<number, number>>({});
  isSubmitting = signal(false);

  ngOnInit(): void {
    forkJoin({
      formContent: this.formService.getFormContent(this.formId),
      answer: this.answerService.getAnswers(this.formId, this.studentId),
    }).subscribe({
      next: ({ formContent, answer }) => {
        if (formContent.success && formContent.payload) {
          this.formContent.set(formContent.payload);
        } else {
          this.toastr.error('Nie udało się pobrać formularza');
        }

        if (answer.success && answer.payload) {
          this.answer.set(answer.payload);
          const map = Object.fromEntries(answer.payload.obligatoryAnswers.map(a => [a.obligatoryPreference.id, a.obligatoryPreferenceOptionId]));
          this.selectedAnswers.set(map);
          this.choosableAnswers.set(answer.payload.choosableAnswers.map(a => a.name));
        } else {
          this.toastr.error('Nie udało się pobrać odpowiedzi');
        }
      },
      error: () => this.toastr.error('Błąd ładowania danych'),
    });
  }

  onSelect(prefId: number, optionId: number): void {
    this.selectedAnswers.update(s => ({ ...s, [prefId]: optionId }));
  }

  
  

  onAddChoosable(): void {
    const input = this.choosableInput.trim();
    if (input) {
      this.choosableAnswers.update(list => [...list, input]);
      this.choosableInput = ''; 
    }
  }
  
  removeChoosable(index: number): void {
    this.choosableAnswers.update(list => list.filter((_, i) => i !== index));
  }

  onDelete(): void {
    const current = this.answer();
    if (!current) return;
  
    if (!confirm('Czy na pewno chcesz usunąć odpowiedzi?')) {
      return;
    }
  
    this.isSubmitting.set(true);
    this.answerService.deleteAnswer(current.id).subscribe({
      next: (res) => {
        this.isSubmitting.set(false);
        if (res.success) {
          this.toastr.success('Usunięto odpowiedzi!');
          this.hide.emit();
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd usuwania', res.error?.title ?? 'Błąd');
        }
      },
      error: () => {
        this.isSubmitting.set(false);
        this.toastr.error('Błąd usuwania');
      }
    });
  }

  openEdit() {
    this.showEdit = true;
  }
  
  hideEdit() {
    this.showEdit = false;
  }
  
  
  
}

