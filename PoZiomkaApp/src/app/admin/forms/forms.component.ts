import { Component, inject, OnInit, signal } from '@angular/core';
import { FormService } from './form.service';
import { ToastrService } from 'ngx-toastr';
import { FormModel } from './form.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forms',
  imports: [],
  templateUrl: './forms.component.html',
  styleUrl: './forms.component.css'
})
export class FormListComponent implements OnInit {
  private formService = inject(FormService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);

  _forms = signal<FormModel[]>([]);
  forms = this._forms.asReadonly();

  ngOnInit(): void {
    this.formService.getForms().subscribe({
      next: response => {
        if (response.success) {
          this._forms.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onFormClick(id: number): void {
    this.router.navigate(['admin', 'form', 'edit', id]);
  }

  onAddFormClick(): void {
    this.router.navigate(['admin', 'form', 'edit', '']);
  }
}
