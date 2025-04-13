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
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";

@Component({
  selector: 'app-application-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ApplicationDetailsComponent, EnumPipe, LoadingButtonComponent],
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
  isLoading = signal<boolean>(false);
  
  filters = signal<ApplicationSearchParams>({
    studentEmail: undefined,
    studentIndex: undefined,
    applicationTypeId: undefined,
    applicationStatus: undefined,
  });
  
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
    
    const params = this.filters();

    Object.keys(params).forEach(key => (params as any)[key] === undefined ? delete (params as any)[key] : {});
    this.applicationService.getApplications(this.filters()).subscribe({
      next: res => {
        if (res.success) {
          this.applications.set(res.payload!);
        } else {
          this.toastr.error(res.error!.detail, res.error!.title);
        }
        this.isLoading.set(false);
      }
    });
  }

  onApplicationClick(app: ApplicationModel): void {
    this.selectedApplicationId.set(app.id);
    this.selectedApplication.set(app);
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
  
  set applicationTypeId(value: number | undefined) {
    this.filters.set({
      ...this.filters(),
      applicationTypeId: value,
    });
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
