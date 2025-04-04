import { Component, inject, input, signal } from '@angular/core';
import { Router } from '@angular/router';
import { StudentService } from '../student/student.service';
import { CardConfiguration, CenteredCardComponent } from '../common/centered-card/centered-card.component';
import { ToastrService } from 'ngx-toastr';
import { LoadingButtonComponent } from "../common/loading-button/loading-button.component";

@Component({
  selector: 'app-confirm-email',
  imports: [CenteredCardComponent, LoadingButtonComponent],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent {
  CardConfiguration = CardConfiguration;

  private router = inject(Router);
  private toastrService = inject(ToastrService);
  private studentService = inject(StudentService);
  token = input.required<string>();

  isLoading = signal<boolean>(false);

  confirmEmail(): void {
    this.isLoading.set(true);
    this.studentService.confirmStudent(this.token()).subscribe({
      next: response => {
        if (response.success) {
          this.router.navigate(['/login']);
        } else {
          this.isLoading.set(false);
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
}