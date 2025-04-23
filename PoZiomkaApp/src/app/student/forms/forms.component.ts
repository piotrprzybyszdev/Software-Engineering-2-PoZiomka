import { Component, inject, OnInit, signal } from '@angular/core';
import { FormService } from '../../form/form.service';
import { FormContentModel, FormModel } from '../../form/form.model';
import { ToastrService } from 'ngx-toastr';
import { FormFillComponent } from './form-fill/form-fill.component';

@Component({
  selector: 'app-forms',
  standalone: true,
  imports: [FormFillComponent],
  templateUrl: './forms.component.html',
  styleUrl: './forms.component.css'
})
export class FormsComponent implements OnInit {
  private formService = inject(FormService);
  private toastr = inject(ToastrService);

  forms = signal<FormModel[]>([]);
  selectedForm = signal<FormContentModel | undefined>(undefined);

  ngOnInit(): void {
    this.formService.getForms().subscribe({
      next: (res) => {
        if (res.success) {
          this.forms.set(res.payload ?? []);
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd', res.error?.title ?? 'Błąd pobierania formularzy');
        }
      }
    });
  }

  onShowForm(form: FormModel): void {
    this.formService.getFormContent(form.id).subscribe({
      next: (res) => {
        if (res.success) {
          this.selectedForm.set(res.payload!);
        } else {
          this.toastr.error(res.error?.detail ?? 'Błąd', res.error?.title ?? 'Błąd pobierania zawartości formularza');
        }
      }
    });
  }

  onHideForm(): void {
    this.selectedForm.set(undefined);
  }
}
