import { Component, EventEmitter, Input, Output, OnInit, inject, signal } from '@angular/core';
import { AnswerService } from '../../answer/answer.service';
import { AnswerModel, AnswerUpdate } from '../../answer/answer.model';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-form-show',
  standalone: true,
  imports: [PopupComponent, CommonModule],
  templateUrl: './form-show.component.html',
  styleUrl: './form-show.component.css'
})
export class FormShowComponent implements OnInit {
  @Input() formId!: number;
  @Input() studentId!: number;
  @Output() hide = new EventEmitter<void>();

  private answerService = inject(AnswerService);
  private toastr = inject(ToastrService);

  answer = signal<AnswerModel | null>(null);
  selectedAnswers = signal<Record<number, number>>({});
  isSubmitting = signal(false);

  ngOnInit(): void {
    this.answerService.getAnswers(this.formId, this.studentId).subscribe({
      next: (res) => {
        if (res.success && res.payload) {
          this.answer.set(res.payload);
          const map = Object.fromEntries(res.payload.ogligatoryAnswers.map(a => [a.obligatoryPreference.id, a.obligatoryPreferenceOptionId]));
          this.selectedAnswers.set(map);
        }
      },
      error: () => this.toastr.error('Nie udało się pobrać odpowiedzi'),
    });
  }

  onSelect(prefId: number, optionId: number): void {
    this.selectedAnswers.update(s => ({ ...s, [prefId]: optionId }));
  }

  onSubmit(): void {
    const current = this.answer();
    if (!current) return;

    const payload: AnswerUpdate = {
      id: current.id,
      obligatoryAnswers: Object.entries(this.selectedAnswers()).map(([prefId, optionId]) => ({
        obligatoryPreferenceId: +prefId,
        obligatoryPreferenceOptionId: optionId,
        isHidden: false
      })),
      choosableAnsers: [], // jeśli nie używamy — zostaje puste
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
      error: () => this.isSubmitting.set(false),
    });
  }
}

