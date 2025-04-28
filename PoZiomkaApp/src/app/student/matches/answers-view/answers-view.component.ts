import { Component, computed, inject, input, OnInit, output, signal } from '@angular/core';
import { PopupComponent } from "../../../common/popup/popup.component";
import { AnswerModel, AnswerStatus } from '../../answers/answer.model';
import { AnswerService } from '../../answers/answer.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-answers-view',
  imports: [PopupComponent],
  templateUrl: './answers-view.component.html',
  styleUrl: './answers-view.component.css'
})
export class AnswersViewComponent implements OnInit{
  studentId = input.required<number>();

  hide = output<void>();

  private answerService = inject(AnswerService);
  private toastrService = inject(ToastrService);

  answers = signal<AnswerStatus[]>([]);
  selectedAnswer = signal<AnswerModel | undefined>(undefined);
  size = computed(() => this.selectedAnswer() === undefined ? 'col-6 p-5' : 'col-10 p-5');

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
      next: response => {
        if (response.success) {
          this.selectedAnswer.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    })
  }

  onAnswerListShow(): void {
    this.selectedAnswer.set(undefined);
  }
}
