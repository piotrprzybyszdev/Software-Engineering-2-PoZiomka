import { Component, OnInit } from '@angular/core';
import { ApplicationModel, ApplicationStatus, ApplicationTypeModel, applicationStatusToString, saveFileToDisk  } from '../../../application/application.model';
import { ApplicationService } from '../../../application/application.service';
import { PopupComponent } from "../../../common/popup/popup.component";
import { ToastrService } from 'ngx-toastr';
import { signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentModel } from '../../../student/student.model';
import { StudentService } from '../../../student/student.service';
import { FormsModule } from '@angular/forms';
import { Input,Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-application-details',
  standalone: true,
  imports: [CommonModule, PopupComponent, FormsModule],
  templateUrl: './application-details.component.html',
})
export class ApplicationDetailsComponent implements OnInit {
  ApplicationStatus = ApplicationStatus;
  selectedApplicationId = signal<number | undefined>(undefined);
  @Input() selectedApplication!: ApplicationModel;
  @Output() hide = new EventEmitter<void>();
  isLoading = signal(false);
  
  studentsMap = signal<Map<number, StudentModel>>(new Map());

  applicationStatusText = applicationStatusToString;


  constructor(
    private applicationService: ApplicationService,
    private toastr: ToastrService,
    private studentService: StudentService
  ) {}

  ngOnInit(): void {
  }

  onHide(): void {
    this.hide.emit();
  }

  // Rozwiązuje aplikację (akceptuje lub odrzuca)
  onApplicationResolve(app: ApplicationModel, status: ApplicationStatus): void {
    this.applicationService.resolveApplication(app.id, status).subscribe({
      next: res => {
        if (res.success) {
          this.toastr.success(`Aplikacja ${status === ApplicationStatus.accepted ? 'zaakceptowana' : 'odrzucona'}`);
          this.onHide();
        } else {
          this.toastr.error(res.error!?.detail, res.error!?.title);
        }
      }
    });
  }

  onDownloadFile(): void {
    const app = this.selectedApplication;
    if (!app) return;
  
    this.applicationService.downloadApplicationFile(app.id).subscribe({
      next: res => {
        if (res.success && res.payload) {
          const blob = new Blob([res.payload], { type: 'application/pdf' });
          saveFileToDisk(blob, `aplikacja_${app.id}.pdf`);
        } else {
          this.toastr.error(res.error?.detail || 'Błąd pobierania pliku');
        }
      },
      error: () => this.toastr.error('Wystąpił błąd przy pobieraniu pliku')
    });
  }
  
  
}