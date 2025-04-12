import { Component, OnInit } from '@angular/core';
import { ApplicationModel, ApplicationStatus, ApplicationTypeModel } from '../../application/application.model';
import { ApplicationService } from '../../application/application.service';
import { PopupComponent } from "../../common/popup/popup.component";
import { ToastrService } from 'ngx-toastr';
import { signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentModel } from '../../student/student.model';
import { StudentService } from '../../student/student.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-application-list',
  standalone: true,
  imports: [CommonModule, PopupComponent, FormsModule],
  templateUrl: './applications.component.html',
})
export class ApplicationListComponent implements OnInit {
  ApplicationStatus = ApplicationStatus;
  applications = signal<ApplicationModel[]>([]);
  selectedApplicationId = signal<number | undefined>(undefined);
  selectedApplication = signal<ApplicationModel | undefined>(undefined);

  applicationTypes: ApplicationTypeModel[] = [];
  isLoading = signal(false);
  
  studentsMap = signal<Map<number, StudentModel>>(new Map());
  filters = signal({
    studentEmail: '',
    applicationType: '',
    applicationStatus: undefined as ApplicationStatus | undefined,
  });



  sortOption = signal<'type' | 'status'>('type');

  constructor(
    private applicationService: ApplicationService,
    private toastr: ToastrService,
    private studentService: StudentService
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
    const filters = this.filters(); 
  
    const params: any = {};
  
    if (filters.studentEmail) {
      params.studentEmail = filters.studentEmail;
    }
  
    if (filters.applicationType) {
      params.applicationType = filters.applicationType;
    }
  
    if (filters.applicationStatus !== undefined) {
      params.applicationStatus = filters.applicationStatus;
    }

  
    this.applicationService.getApplications(params).subscribe({
      next: res => {
        this.isLoading.set(false);
        if (res.success) {
          this.applications.set(res.payload!);          
          this.loadStudents();
        } else {
          this.toastr.error(res.error?.detail, res.error?.title);
        }
      },
      error: () => this.isLoading.set(false)
    });
  }
  
  

  loadStudents(): void {
    const applicationList = this.applications();
    const uniqueStudentIds = Array.from(new Set(applicationList.map(app => app.studentId)));
    const map = new Map<number, StudentModel>();
    if (uniqueStudentIds.length === 0) {
      this.studentsMap.set(map); 
      return;
    }
  
    let loadedCount = 0;
  
    uniqueStudentIds.forEach(id => {
      this.studentService.getStudent(id).subscribe({
        next: res => {
          loadedCount++;  
          if (res.success && res.payload) {
            map.set(id, res.payload);
          } 
  
          if (loadedCount === uniqueStudentIds.length) {
            this.studentsMap.set(map);
          }
        },
        error: () => {
          loadedCount++;
          if (loadedCount === uniqueStudentIds.length) {
            this.studentsMap.set(map);
          }
          this.toastr.error(`Błąd podczas pobierania studenta o ID ${id}`);
        }
      });
    });
  }
  
  

  filteredApplications() {
    //this.loadStudents();
    return this.applications().filter(application => {
      const filters = this.filters();
      let matches = true;

  
      const student = this.studentsMap().get(application.studentId);

      if (filters.studentEmail && (!student || !student.email.toLowerCase().includes(filters.studentEmail.toLowerCase()))) {
        matches = false;
      }

      if (filters.applicationType && !application.applicationType.name.toLowerCase().includes(filters.applicationType.toLowerCase())) {
        matches = false;
      }
  
      if (filters.applicationStatus !== undefined && application.applicationStatus !== filters.applicationStatus) {
        matches = false;
      }
      return matches;
    });
  }
  

  // Funkcja grupująca aplikacje na podstawie wybranego parametru (typ lub status)
  groupedApplications() {
    const sortedApplications = this.filteredApplications().sort((a, b) => {
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

  // Obsługuje kliknięcie aplikacji
  onApplicationClick(app: ApplicationModel): void {
    this.selectedApplicationId.set(app.id);
    this.selectedApplication.set(app);
  }

  // Zwraca nazwę statusu aplikacji
  applicationStatusText(status: ApplicationStatus): string {
    switch (status) {
      case ApplicationStatus.pending: return 'Oczekująca';
      case ApplicationStatus.accepted: return 'Zaakceptowana';
      case ApplicationStatus.rejected: return 'Odrzucona';
      default: return 'Nieznany';
    }
  }

  // Ustawia opcję sortowania
  setSortOption(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.sortOption.set(target.value as 'type' | 'status');
  }

  // Rozwiązuje aplikację (akceptuje lub odrzuca)
  onApplicationResolve(app: ApplicationModel, status: ApplicationStatus): void {
    this.applicationService.resolveApplication(app.id, status).subscribe({
      next: res => {
        if (res.success) {
          this.toastr.success(`Aplikacja ${status === ApplicationStatus.accepted ? 'zaakceptowana' : 'odrzucona'}`);
          this.loadApplications();
          this.loadStudents();
          this.onHide();
        } else {
          this.toastr.error(res.error!?.detail, res.error!?.title);
        }
      }
    });
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



  get studentEmail() {
    return this.filters().studentEmail;
  }
  set studentEmail(value: string) {
    this.filters.set({
      ...this.filters(),
      studentEmail: value,
    });
  }
  
  get applicationType() {
    return this.filters().applicationType;
  }
  set applicationType(value: string) {
    this.filters.set({
      ...this.filters(),
      applicationType: value,
    });
  }
  
  get applicationStatus() {
    return this.filters().applicationStatus;
  }
  set applicationStatus(value: ApplicationStatus | undefined) {
    this.filters.set({
      ...this.filters(),
      applicationStatus: value as ApplicationStatus,
    });
  }
  
}
