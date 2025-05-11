import { Component, EventEmitter, OnInit, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommunicationService } from '../../../communication/communication.service';
import { CommunicationModel } from '../../../communication/communication.model';
import { ToastrService } from 'ngx-toastr';
import { PopupComponent } from '../../../common/popup/popup.component';

@Component({
  selector: 'app-student-communication-popup',
  standalone: true,
  imports: [CommonModule, PopupComponent],
  templateUrl: './communication.component.html',
  styleUrl: './communication.component.css'
})
export class StudentCommunicationPopupComponent implements OnInit {
  private communicationService = inject(CommunicationService);
  private toastr = inject(ToastrService);

  communications = signal<CommunicationModel[]>([]);
  isLoading = signal(false);

  @Output() hide = new EventEmitter<void>();

  ngOnInit(): void {
    this.loadCommunications();
  }

  loadCommunications(): void {
    this.communicationService.getStudentCommunication().subscribe({
      next: res => {
        if (res.success) {
          this.communications.set(res.payload ?? []);
        } else {
          this.toastr.error(res.error?.detail || 'Błąd ładowania komunikatów');
        }
      }
    });
  }

  deleteCommunication(id: number): void {
    if (!confirm("Czy na pewno chcesz usunąć ten komunikat?")) return;

    this.communicationService.deleteCommunication(id).subscribe({
      next: res => {
        if (res.success) {
          this.toastr.success("Komunikat usunięty");
          this.communications.update(list => list.filter(msg => msg.id !== id));
        } else {
          this.toastr.error(res.error?.detail || 'Nie udało się usunąć');
        }
      }
    });
  }

  onHide(): void {
    this.hide.emit();
  }
}
