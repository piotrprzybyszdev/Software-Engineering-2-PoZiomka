import { Component, EventEmitter, Input, Output, OnInit, inject, signal } from '@angular/core';
import { AnswerService } from '../../answer/answer.service';
import { FormService } from '../../../form/form.service';
import { AnswerModel, AnswerUpdate } from '../../answer/answer.model';
import { FormContentModel } from '../../../form/form.model';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-show',
  standalone: true,
  imports: [PopupComponent, CommonModule, FormsModule],
  templateUrl: './form-show.component.html',
  styleUrl: './form-show.component.css'
})
export class FormShowComponent implements OnInit {
  @Input() formId!: number;
  @Input() studentId!: number;
  @Output() hide = new EventEmitter<void>();

  private answerService = inject(AnswerService);
  private formService = inject(FormService);
  private toastr = inject(ToastrService);

  choosableInput = '';
  choosableAnswers = signal<string[]>([]); 


  formContent = signal<FormContentModel | null>(null);
  answer = signal<AnswerModel | null>(null);
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
          const map = Object.fromEntries(answer.payload.ogligatoryAnswers.map(a => [a.obligatoryPreference.id, a.obligatoryPreferenceOptionId]));
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

  onSubmit(): void {
    const currentAnswer = this.answer();
    if (!currentAnswer) return;
  
    const payload: AnswerUpdate = {
      id: currentAnswer.id,
      obligatoryAnswers: Object.entries(this.selectedAnswers()).map(([prefId, optionId]) => ({
        obligatoryPreferenceId: +prefId,
        obligatoryPreferenceOptionId: optionId,
        isHidden: false,
      })),
      choosableAnsers: this.choosableAnswers().map(name => ({
        name,
        isHidden: false,
      })),
    };
  
    this.isSubmitting.set(true);
    this.answerService.updateAnswer(payload).subscribe({
      next: (res) => {
        this.isSubmitting.set(false);
        if (res.success) {
          this.toastr.success('Zaktualizowano odpowiedzi!');
          this.hide.emit();
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd zapisu', res.error?.title ?? 'Błąd');
        }
      },
      error: () => {
        this.isSubmitting.set(false);
        this.toastr.error('Błąd zapisu');
      }
    });
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
  
  
}

