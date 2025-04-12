import { Component, computed, inject, input, output, signal } from '@angular/core';
import { PopupComponent } from '../../../common/popup/popup.component';
import { ApplicationModel, applicationStatusToColorString, applicationStatusToString, ApplicationTypeModel, saveFileToDisk } from '../../../application/application.model';
import { ApplicationService } from '../../../application/application.service';
import { ToastrService } from 'ngx-toastr';
import { LoadingButtonComponent } from "../../../common/loading-button/loading-button.component";

@Component({
  selector: 'app-application-list',
  imports: [PopupComponent, LoadingButtonComponent],
  templateUrl: './application-list.component.html',
  styleUrl: './application-list.component.css'
})
export class ApplicationListComponent {
  applicationType = input.required<ApplicationTypeModel>();

  hide = output<void>();

  private applicationService = inject(ApplicationService);
  private toastrService = inject(ToastrService);

  applications = signal<ApplicationModel[]>([]);

  applicationStatusToString = applicationStatusToString;
  applicationStatusToColorString = applicationStatusToColorString;

  isUploading = signal<boolean>(false);
  isDownloading = signal<boolean[]>([]);
  isDownloadingAny = computed(() => !this.isDownloading().every(b => b === false));

  constructor() {
    this.refreshApplications();
  }

  onHide(): void {
    if (this.isUploading()) {
      return;
    }

    this.hide.emit();
  }

  updateArr(arr: boolean[], index: number, value: boolean): boolean[] {
    const narr = [...arr];
    narr[index] = value;
    return narr;
  }

  onUploadFile(event: Event): void {
    const target = event.target as HTMLInputElement;
    const file = target.files?.item(0);

    if (!file) {
      return;
    }

    this.isUploading.set(true);
    this.applicationService.submitApplication(this.applicationType().id, file).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie utworzono wniosek!');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
          this.refreshApplications();
        }
        this.isUploading.set(false);
      }
    });
  }

  onDownloadFile(applicationIndex: number): void {
    const application = this.applications()[applicationIndex];

    this.isDownloading.update(arr => this.updateArr(arr, applicationIndex, true));
    this.applicationService.downloadApplicationFile(application.id).subscribe({
      next: response => {
        if (response.success) {
          saveFileToDisk(response.payload!, `${this.applicationType().name}-${application.id}.pdf`);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isDownloading.update(arr => this.updateArr(arr, applicationIndex, false));
      }
    });
  }

  private refreshApplications(): void {
    this.applicationService.getStudentApplications().subscribe({
      next: response => {
        if (response.success) {
          this.applications.set(response.payload!);
          this.isDownloading.set(Array(response.payload!.length).fill(false));
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    })
  }
}
