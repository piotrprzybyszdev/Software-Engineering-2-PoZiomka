import { Component, signal, inject, output, input, ɵIS_INCREMENTAL_HYDRATION_ENABLED } from '@angular/core';
import { ObligatoryAnswerCreate, AnswerModel, AnswerUpdate, ChoosableAnswerCreate } from '../answer.model';
import { AnswerService } from '../answer.service';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StudentService } from '../../student.service';
import { FormModel } from '../../../admin/forms/form.model';

@Component({
  selector: 'app-answer-edit',
  standalone: true,
  imports: [PopupComponent, CommonModule, FormsModule],
  templateUrl: './answer-edit.component.html',
  styleUrl: './answer-edit.component.html'
})
export class AnswerEditComponent {
  form = input.required<FormModel>();
  save = output<void>();
  hide = output<void>();

  private answerService = inject(AnswerService);
  private studentService = inject(StudentService);
  private toastr = inject(ToastrService);

  choosableInput = '';

  answer = signal<AnswerModel | undefined>(undefined);

  isSubmitting = signal(false);

  ngOnInit(): void {
    this.loadAnswer();
  }

  loadAnswer(): void {
    this.answerService.getAnswers(this.form().id, this.studentService.loggedInStudent()!.id).subscribe({
      next: response => {
        if (response.success) {
          this.answer.set(response.payload);
        } else {
          this.toastr.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
  
  onOptionSelect(answerIndex: number, optionId: number): void {
    this.answer.update(value => {
      if (value === undefined) {
        return undefined;
      }

      const oldAnswer = value.obligatoryAnswers[answerIndex];

      return {
        id: value.id,
        formId: value.formId,
        studentId: value.studentId,
        status: value.status,
        choosableAnswers: value.choosableAnswers,
        obligatoryAnswers: value.obligatoryAnswers.updateClone(answerIndex, {
          id: oldAnswer.id,
          obligatoryPreference: oldAnswer.obligatoryPreference,
          obligatoryPreferenceOptionId: optionId,
          isHidden: oldAnswer.isHidden
        })
      }
    })
  }

  onAddChoosable(): void {
    const trimmed = this.choosableInput.trim();
    if (trimmed) {
      this.answer.update(value => {
        if (value === undefined) {
          return undefined;
        }

        return {
          id: value.id,
          formId: value.formId,
          studentId: value.studentId,
          status: value.status,
          choosableAnswers: [...value.choosableAnswers, {
            id: 0,
            name: trimmed,
            isHidden: false
          }],
          obligatoryAnswers: value.obligatoryAnswers
        }
      });

      this.choosableInput = '';
    }
  }
  
  removeChoosable(index: number): void {
    this.answer.update(value => {
      if (value === undefined) {
        return undefined;
      }

      return {
        id: value.id,
        formId: value.formId,
        studentId: value.studentId,
        status: value.status,
        choosableAnswers: value.choosableAnswers.filter((_, i) => i !== index),
        obligatoryAnswers: value.obligatoryAnswers
      }
    });
  }

  toggleObligatoryVisiblity(index: number): void {
    this.answer.update(value => {
      if (value === undefined) {
        return undefined;
      }

      return {
        id: value.id,
        formId: value.formId,
        studentId: value.studentId,
        status: value.status,
        choosableAnswers: value.choosableAnswers,
        obligatoryAnswers: value.obligatoryAnswers.map((answer, i) => {
          if (i !== index) {
            return answer;
          }

          return {
            id: answer.id,
            obligatoryPreference: answer.obligatoryPreference,
            obligatoryPreferenceOptionId: answer.obligatoryPreferenceOptionId,
            isHidden: !answer.isHidden
          };
        })
      }
    });
  }

  toggleChoosableVisibility(index: number): void {
    this.answer.update(value => {
      if (value === undefined) {
        return undefined;
      }

      return {
        id: value.id,
        formId: value.formId,
        studentId: value.studentId,
        status: value.status,
        choosableAnswers: value.choosableAnswers.map((answer, i) => {
          if (i !== index) {
            return answer;
          }

          return {
            id: answer.id,
            name: answer.name,
            isHidden: !answer.isHidden
          };
        }),
        obligatoryAnswers: value.obligatoryAnswers
      }
    });
  }

  onSubmit(): void {
    const newAnswer = {
      formId: this.answer()!.formId,
      choosableAnswers: this.answer()!.choosableAnswers.map<ChoosableAnswerCreate>(answer => {
        return {
          name: answer.name,
          isHidden: answer.isHidden
        };
      }),
      obligatoryAnswers: this.answer()!.obligatoryAnswers
        .filter(answer => answer.obligatoryPreferenceOptionId !== null)
        .map<ObligatoryAnswerCreate>(answer => {
        return {
          obligatoryPreferenceId: answer.obligatoryPreference.id,
          obligatoryPreferenceOptionId: answer.obligatoryPreferenceOptionId!,
          isHidden: answer.isHidden
        };
      })
    };

    if (this.answer()!.id === null) {
      this.answerService.createAnswer(newAnswer).subscribe({
        next: response => {
          if (response.success) {
            this.toastr.success('Zapisano odpowiedzi');
            this.loadAnswer();
            this.save.emit();
          } else {
            this.toastr.error(response.error!.detail, response.error!.title);
          }
        }
      });
    } else {
      this.answerService.updateAnswer(newAnswer).subscribe({
        next: response => {
          if (response.success) {
            this.toastr.success('Zapisano odpowiedzi');
            this.loadAnswer();
            this.save.emit();
          } else {
            this.toastr.error(response.error!.detail, response.error!.title);
          }
        }
      });
    }
  }
}
