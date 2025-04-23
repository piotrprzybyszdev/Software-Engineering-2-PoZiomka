import { Component, EventEmitter, Input, Output, signal, inject } from '@angular/core';
import { FormContentModel } from '../../../form/form.model';
import { AnswerCreate, ObligatoryAnswerCreate } from '../../answer/answer.model';
import { AnswerService } from '../../answer/answer.service';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-fill',
  standalone: true,
  imports: [PopupComponent, CommonModule, FormsModule],
  templateUrl: './form-fill.component.html',
  styleUrl: './form-fill.component.html'
})
export class FormFillComponent {
  @Input() formContent!: FormContentModel;
  @Output() hide = new EventEmitter<void>();

  private answerService = inject(AnswerService);
  private toastr = inject(ToastrService);

  choosableInput = '';
  choosableAnswers = signal<string[]>([]);

  selectedOptions = signal<Record<number, number>>({}); // preferenceId -> optionId

  isSubmitting = signal(false);
  missingObligatoryAnswers = signal(false);

  onOptionSelect(prefId: number, optionId: number): void {
    this.selectedOptions.update((s) => ({ ...s, [prefId]: optionId }));
  }

  onAddChoosable(): void {
    const trimmed = this.choosableInput.trim();
    if (trimmed) {
      this.choosableAnswers.update((answers) => [...answers, trimmed]);
      this.choosableInput = '';
    }
  }
  
  removeChoosable(index: number): void {
    this.choosableAnswers.update((answers) => answers.filter((_, i) => i !== index));
  }

  onSubmit(): void {
    const selected = this.selectedOptions();
  
    // Sprawdź, czy wszystkie obligatory mają odpowiedź
    const missing = this.formContent.obligatoryPreferences.some(pref => !selected[pref.id]);
    if (missing) {
      this.missingObligatoryAnswers.set(true);
      return;
    }
  
    this.missingObligatoryAnswers.set(false);
  
    const obligatoryAnswers: ObligatoryAnswerCreate[] = Object.entries(selected).map(([prefId, optionId]) => ({
      obligatoryPreferenceId: +prefId,
      obligatoryPreferenceOptionId: optionId,
      isHidden: false,
    }));
  
    const payload: AnswerCreate = {
      formId: this.formContent.id,
      obligatoryAnswers,
      choosableAnswers: this.choosableAnswers().map(name => ({
        name,
        isHidden: false
      }))
    };
  
    this.isSubmitting.set(true);
    this.answerService.createAnswer(payload).subscribe({
      next: (res) => {
        this.isSubmitting.set(false);
        if (res.success) {
          this.toastr.success('Formularz zapisany pomyślnie!');
          this.hide.emit();
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd zapisu', res.error?.title ?? 'Błąd');
        }
      },
      error: () => this.isSubmitting.set(false),
    });
  }
  
}
