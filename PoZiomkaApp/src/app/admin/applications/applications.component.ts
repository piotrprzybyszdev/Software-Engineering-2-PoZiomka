import { Component, OnInit } from '@angular/core';
import { ApplicationModel, ApplicationStatus, ApplicationTypeModel } from '../../application/application.model';
import { ApplicationService } from '../../application/application.service';
import { PopupComponent } from "../../common/popup/popup.component";
import { ToastrService } from 'ngx-toastr';
import { signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-application-list',
  standalone: true,
  imports: [CommonModule,PopupComponent],
  templateUrl: './applications.component.html',
})
export class ApplicationListComponent implements OnInit {
  ApplicationStatus = ApplicationStatus;
  applications = signal<ApplicationModel[]>([]);
  selectedApplicationId = signal<number | undefined>(undefined);
  selectedApplication = signal<ApplicationModel | undefined>(undefined);

  applicationTypes: ApplicationTypeModel[] = [];
  isLoading = signal(false);
  filters = signal<any>({});

  sortOption = signal<'type' | 'status'>('type');

  constructor(
    private applicationService: ApplicationService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadApplications();
    this.applicationService.getApplicationTypes().subscribe({
        next: res => {
          if (res.success) this.applicationTypes = res.payload!;
        }
      });
  }

  loadApplications(): void {
    this.isLoading.set(true);
    const params = this.filters(); 
  
    this.applicationService.getApplications(params).subscribe({
      next: res => {
        this.isLoading.set(false);
        if (res.success) {
          this.applications.set(res.payload!);
        } else {
          this.toastr.error(res.error!?.detail, res.error!?.title);
        }
      },
      error: () => this.isLoading.set(false)
    });
  }
  


  groupedApplications() {
    const sortedApplications = this.applications().sort((a, b) => {
      if (this.sortOption() === 'type') {
        return a.applicationType.name.localeCompare(b.applicationType.name);
      } else {
        return a.applicationStatus - b.applicationStatus;
      }
    });

    const groups: { [key: string]: ApplicationModel[] } = {};

    sortedApplications.forEach(app => {
      const key = this.sortOption() === 'type' ? app.applicationType.name : this.applicationStatusText(app.applicationStatus);
      if (!groups[key]) {
        groups[key] = [];
      }
      groups[key].push(app);
    });

    return Object.keys(groups).map(key => ({ key, applications: groups[key] }));
  }

  onApplicationClick(app: ApplicationModel): void {
    this.selectedApplicationId.set(app.id);
    this.selectedApplication.set(app);
  }

  applicationStatusText(status: ApplicationStatus): string {
    switch (status) {
      case ApplicationStatus.pending: return 'Oczekująca';
      case ApplicationStatus.accepted: return 'Zaakceptowana';
      case ApplicationStatus.rejected: return 'Odrzucona';
      default: return 'Nieznany';
    }
  }

  setSortOption(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.sortOption.set(target.value as 'type' | 'status');
  }

  onApplicationResolve(app: ApplicationModel, status: ApplicationStatus): void {
    this.applicationService.resolveApplication(app.id, status).subscribe({
      next: res => {
        if (res.success) {
          this.toastr.success(`Aplikacja ${status === ApplicationStatus.accepted ? 'zaakceptowana' : 'odrzucona'}`);
          this.loadApplications();
          this.onHide();
        } else {
          this.toastr.error(res.error!?.detail, res.error!?.title);
        }
      }
    });
  }
  

  // do grupowania po nazwie (bo nie łapie spacji) -> zamienić na jakieś id?
  groupKeySafe(key: string): string {
    return key.toLowerCase()
              .replace(/ą/g, 'a').replace(/ć/g, 'c').replace(/ę/g, 'e')
              .replace(/ł/g, 'l').replace(/ń/g, 'n').replace(/ó/g, 'o')
              .replace(/ś/g, 's').replace(/ź/g, 'z').replace(/ż/g, 'z')
              .replace(/\s+/g, '-') // spacje na "-"
              .replace(/[^a-z0-9\-]/g, ''); // usuń inne znaki
  }

  onHide(): void {
    this.selectedApplication.set(undefined);
    this.selectedApplicationId.set(undefined);
  }
  
  onDownloadFile(): void {
    const app = this.selectedApplication();
    if (!app) return;
  
    this.applicationService.downloadApplicationFile(app.id).subscribe({
      next: res => {
        if (res.success && res.payload) {
          const blob = new Blob([res.payload], { type: 'application/pdf' });
          const url = window.URL.createObjectURL(blob);
  
          const a = document.createElement('a');
          a.href = url;
          a.download = `aplikacja_${app.id}.pdf`;
          a.click();
  
          window.URL.revokeObjectURL(url);
        } else {
          this.toastr.error(res.error?.detail || 'Błąd pobierania pliku');
        }
      },
      error: () => this.toastr.error('Wystąpił błąd przy pobieraniu pliku')
    });
  }
  
  
}