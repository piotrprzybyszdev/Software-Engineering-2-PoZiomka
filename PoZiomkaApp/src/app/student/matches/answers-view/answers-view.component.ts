import { Component, computed, inject, input, OnInit, output, signal } from '@angular/core';
import { PopupComponent } from "../../../common/popup/popup.component";
import { AnswerModel, AnswerStatus } from '../../answers/answer.model';
import { AnswerService } from '../../answers/answer.service';
import { ToastrService } from 'ngx-toastr';
import { FormContentModel } from '../../../admin/forms/form.model';
import { FormService } from '../../../admin/forms/form.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-answers-view',
  imports: [PopupComponent, CommonModule],
  templateUrl: './answers-view.component.html',
  styleUrl: './answers-view.component.css'
})
export class AnswersViewComponent implements OnInit{
  studentId = input.required<number>();

  hide = output<void>();

  private answerService = inject(AnswerService);
  private toastrService = inject(ToastrService);

  private formService = inject(FormService);
  selectedFormContent = signal<FormContentModel | undefined>(undefined);

  answers = signal<AnswerStatus[]>([]);
  selectedAnswer = signal<AnswerModel | undefined>(undefined);
  size = computed(() => this.selectedAnswer() === undefined ? 'col-6 p-5' : 'col-10 p-5');
  filteredChoosableAnswers = computed(() =>
    this.selectedAnswer()?.choosableAnswers?.filter(a => !a.isHidden) || []
  );
  

  ngOnInit(): void {
    this.answerService.getStudentAnswers(this.studentId()).subscribe({
      next: response => {
        if (response.success) {
          this.answers.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onHide(): void {
    this.hide.emit();
  }

  onAnswerSelect(formId: number): void {
    this.answerService.getAnswers(formId, this.studentId()).subscribe({
      next: answerResponse => {
        if (answerResponse.success) {
          this.selectedAnswer.set(answerResponse.payload!);

          this.formService.getFormContent(formId).subscribe({
            next: formResponse => {
              if (formResponse.success) {
                this.selectedFormContent.set(formResponse.payload!);
              } else {
                this.toastrService.error(formResponse.error!.detail, formResponse.error!.title);
              }
            }
          });

        } else {
          this.toastrService.error(answerResponse.error!.detail, answerResponse.error!.title);
        }
      }
    });
  }

  onAnswerListShow(): void {
    this.selectedAnswer.set(undefined);
    this.selectedFormContent.set(undefined);
  }

  getObligatoryAnswerName(prefId: number, optionId: number | null): string {
    const pref = this.selectedFormContent()?.obligatoryPreferences.find(p => p.id === prefId);
    const option = pref?.options.find(o => o.id === optionId);
    return option?.name || 'Brak odpowiedzi';
  }
  
}
