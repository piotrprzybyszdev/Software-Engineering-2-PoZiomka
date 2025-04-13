import { Component, OnInit } from '@angular/core';
import { ApplicationModel, ApplicationStatus, ApplicationSearchParams, ApplicationTypeModel, applicationStatusToColorString, applicationStatusToString} from '../../application/application.model';
import { ApplicationService } from '../../application/application.service';
import { ToastrService } from 'ngx-toastr';
import { signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentModel } from '../../student/student.model';
import { StudentService } from '../../student/student.service';
import { FormsModule } from '@angular/forms';
import { ApplicationDetailsComponent } from './application-details/application-details.component';
import { EnumPipe } from "../../common/enum-pipe";

@Component({
  selector: 'app-application-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ApplicationDetailsComponent, EnumPipe],
  templateUrl: './applications.component.html',
})
export class ApplicationListComponent implements OnInit {
  ApplicationStatus = ApplicationStatus;
  applications = signal<ApplicationModel[]>([]);
  selectedApplicationId = signal<number | undefined>(undefined);
  selectedApplication = signal<ApplicationModel | undefined>(undefined);
  applicationStatusText = applicationStatusToString;
  applicationStatusColor = applicationStatusToColorString;

  applicationTypes: ApplicationTypeModel[] = [];
  isLoading = signal(false);
  
  studentsMap = signal<Map<number, StudentModel>>(new Map());
  filters = signal<ApplicationSearchParams>({
    studentEmail: undefined,
    studentIndex: undefined,
    applicationTypeId: undefined,
    applicationStatus: undefined,
  });
  

  sortOption = signal<'type' | 'status'>('type');

  constructor(
    private applicationService: ApplicationService,
    private toastr: ToastrService,
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
    console.log('Sending filters to backend:', this.filters());
  
    this.applicationService.getApplications(this.filters()).subscribe({
      next: res => {
        this.isLoading.set(false);
        if (res.success) {
          this.applications.set(res.payload!);
        } else {
          this.toastr.error(res.error?.detail, res.error?.title);
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

  setSortOption(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.sortOption.set(target.value as 'type' | 'status');
  }

  onHide(): void {
    this.selectedApplication.set(undefined);
    this.selectedApplicationId.set(undefined);
  }


  get studentEmail() {
    return this.filters().studentEmail ?? '';
  }
  set studentEmail(value: string) {
    this.filters.set({
      ...this.filters(),
      studentEmail: value?.trim() || undefined,
    });
  }
  
  get applicationTypeId() {
    return this.filters().applicationTypeId;
  }
  set applicationTypeId(value: number | undefined) {
    this.filters.set({
      ...this.filters(),
      applicationTypeId: value,
    });
  }
  
  get applicationStatus() {
    return this.filters().applicationStatus;
  }
  set applicationStatus(value: ApplicationStatus | undefined) {
    this.filters.set({
      ...this.filters(),
      applicationStatus: value,
    });
  }

  get studentIndex() {
    return this.filters().studentIndex ?? '';
  }
  set studentIndex(value: string) {
    this.filters.set({
      ...this.filters(),
      studentIndex: value?.trim() || undefined,
    });
  }  
  
  
}
