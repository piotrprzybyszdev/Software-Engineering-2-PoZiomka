import { Component, inject, OnInit, signal } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';
import { AnswerStatus, FormStatus } from './answer.model';
import { AnswerService } from './answer.service';
import { CommonModule } from '@angular/common';
import { FormModel } from '../../admin/forms/form.model';

@Component({
  selector: 'app-answers',
  standalone: true,
  imports: [AnswerEditComponent, CommonModule],
  templateUrl: './answers.component.html',
  styleUrl: './answers.component.css'
})
export class AnswersComponent implements OnInit {
  private answerService = inject(AnswerService);
  private toastr = inject(ToastrService);

  answers = signal<AnswerStatus[]>([]);
  selectedForm = signal<FormModel | undefined>(undefined);

  ngOnInit(): void {
    this.loadAnswers();
  }

  loadAnswers(): void {
    this.answerService.getStudentAnswers().subscribe({
      next: (res) => {
        if (res.success) {
          this.answers.set(res.payload!);
        } else {
          this.toastr.error(res.error!.detail, res.error!.title);
        }
      }
    });
  }
  
  getActionLabel(status: FormStatus): string {
    switch (status) {
      case FormStatus.NotFilled:
        return 'Wypełnij ankietę';
      case FormStatus.InProgress:
        return 'Kontynuuj wypełnianie';
      case FormStatus.Filled:
        return 'Pokaż odpowiedzi';
    }
  }

  getButtonClass(status: FormStatus): string {
    switch (status) {
      case FormStatus.NotFilled:
        return 'btn btn-primary me-2'; 
      case FormStatus.InProgress:
        return 'btn btn-warning me-2'; 
      case FormStatus.Filled:
        return 'btn btn-success me-2'; 
    }
  }
  
  onShowForm(form: FormModel): void {
    this.selectedForm.set(form);
  }

  onSave(): void {
    this.loadAnswers();
  }
  
  onHideForm(): void {
    this.selectedForm.set(undefined);
    this.loadAnswers();
  }
}

