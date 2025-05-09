import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { StudentService } from '../../student/student.service';
import { ToastrService } from 'ngx-toastr';
import { StudentModel } from '../../student/student.model';
import { FormsModule } from '@angular/forms';
import { PopupComponent } from "../../common/popup/popup.component";
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";
import { CommunicationService } from '../../communication/communication.service';

@Component({
  selector: 'app-communications',
  imports: [FormsModule, PopupComponent, LoadingButtonComponent],
  templateUrl: './communications.component.html',
  styleUrl: './communications.component.css'
})
export class CommunicationsComponent implements OnInit {
  private communicationService = inject(CommunicationService);
  private studentService = inject(StudentService);
  private toastrService = inject(ToastrService);

  private _students = signal<StudentModel[]>([]);

  students = computed(() => this._students().filter(student =>
    student.email.startsWith(this.email()) &&
    student.indexNumber?.startsWith(this.indexNumber()) &&
    student.phoneNumber?.startsWith(this.phoneNumber()) &&
    student.firstName?.startsWith(this.firstName()) &&
    student.lastName?.startsWith(this.lastName())));
  isSelected = signal<boolean[]>([]);
  allSelected = computed(() => this.isSelected().every(b => b === true));
  isShowingPopup = signal<boolean>(false);
  isSending = signal<boolean>(false);

  firstName = signal<string>('');
  lastName = signal<string>('');
  email = signal<string>('');
  indexNumber = signal<string>('');
  phoneNumber = signal<string>('');

  description = signal<string>('');

  ngOnInit(): void {
    this.studentService.getAllStudents().subscribe({
      next: response => {
        if (response.success) {
          this._students.set(response.payload!);
          this.isSelected.set(Array(response.payload!.length).fill(false));
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onToggleSelectAll(): void {
    this.isSelected.set(Array(this.students().length).fill(!this.allSelected()));
  }

  onToggleSelected(index: number): void {
    this.isSelected.update(value => value.updateClone(index, !value[index]));
  }

  onOpenCommunicationPopup(): void {
    this.isShowingPopup.set(true);
    this.description.set('');
  }

  onHidePopup(): void {
    this.isShowingPopup.set(false);
  }

  onSend(): void {
    this.isSending.set(true);
    this.communicationService.sendCommunication(
      [...this.isSelected().keys()].filter(index => this.isSelected()[index]), this.description()
    ).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie wysłano komunikat');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isSending.set(false);
      }
    });
  }
}
