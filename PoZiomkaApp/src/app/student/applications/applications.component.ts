import { Component, inject, OnInit, signal } from '@angular/core';
import { ApplicationService } from '../../application/application.service';
import { ApplicationTypeModel } from '../../application/application.model';
import { ToastrService } from 'ngx-toastr';
import { ApplicationListComponent } from './application-list/application-list.component';

@Component({
  selector: 'app-applications',
  imports: [ApplicationListComponent],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent implements OnInit {
  applicationService = inject(ApplicationService);
  toastrService = inject(ToastrService);

  applicationTypes = signal<ApplicationTypeModel[]>([]);

  listApplicationType = signal<ApplicationTypeModel | undefined>(undefined);

  ngOnInit(): void {
    this.applicationService.getApplicationTypes().subscribe({
      next: response => {
        if (response.success) {
          this.applicationTypes.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onShowApplicationList(applicationType: ApplicationTypeModel): void {
    this.listApplicationType.set(applicationType);
  }

  onHideApplicationList(): void {
    this.listApplicationType.set(undefined);
  }
}
