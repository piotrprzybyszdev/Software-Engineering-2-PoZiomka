import { Component, EventEmitter, Input, Output, signal, inject } from '@angular/core';
import { FormContentModel } from '../../../form/form.model';
import { AnswerModel, AnswerCreate, ObligatoryAnswerCreate, ChoosableAnswerCreate, AnswerUpdate } from '../../answer/answer.model';
import { AnswerService } from '../../answer/answer.service';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-answer-edit',
  standalone: true,
  imports: [PopupComponent, CommonModule, FormsModule],
  templateUrl: './answer-edit.component.html',
  styleUrl: './answer-edit.component.html'
})
export class AnswerEditComponent {
  @Input() formContent!: FormContentModel;
  @Input() existingAnswer?: AnswerModel;
  @Output() hide = new EventEmitter<void>();

  private answerService = inject(AnswerService);
  private toastr = inject(ToastrService);

  choosableInput = '';
  choosableAnswers = signal<string[]>([]);

  selectedOptions = signal<Record<number, number>>({}); 

  isSubmitting = signal(false);
  missingObligatoryAnswers = signal(false);

  ngOnInit(): void {
    if (this.existingAnswer) {
      const selected: Record<number, number> = {};
      for (const ans of this.existingAnswer.obligatoryAnswers) {
        selected[ans.obligatoryPreference.id] = ans.obligatoryPreferenceOptionId;
      }
      this.selectedOptions.set(selected);
  
      const choosable = this.existingAnswer.choosableAnswers.map(a => a.name);
      this.choosableAnswers.set(choosable);
    }
  }
  
  

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
  
    const choosableAnswers: ChoosableAnswerCreate[] = this.choosableAnswers().map(name => ({
      name,
      isHidden: false
    }));
  
    this.isSubmitting.set(true);
  
    if (this.existingAnswer) {
      const payload: AnswerUpdate = {
        id: this.existingAnswer.id,
        obligatoryAnswers,
        choosableAnswers
      };
  
      this.answerService.updateAnswer(payload).subscribe({
        next: (res) => {
          this.isSubmitting.set(false);
          if (res.success) {
            this.toastr.success('Odpowiedź zaktualizowana pomyślnie!');
            this.hide.emit();
          } else {
            this.toastr.error(res.error?.detail ?? 'Błąd aktualizacji', res.error?.title ?? 'Błąd');
          }
        },
        error: () => this.isSubmitting.set(false),
      });
    } else {
      const payload: AnswerCreate = {
        formId: this.formContent.id,
        obligatoryAnswers,
        choosableAnswers
      };
  
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
  
  
}
