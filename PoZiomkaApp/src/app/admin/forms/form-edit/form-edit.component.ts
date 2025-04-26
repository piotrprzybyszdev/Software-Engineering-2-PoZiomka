import { Component, inject, input, OnInit, signal } from '@angular/core';
import { FormUpdate, ObligatoryPreferenceCreate } from '../form.model';
import { FormService } from '../form.service';
import { ToastrService } from 'ngx-toastr';
import { PreferenceEditComponent } from "./preference-edit/preference-edit.component";
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-edit',
  imports: [PreferenceEditComponent, FormsModule],
  templateUrl: './form-edit.component.html',
  styleUrl: './form-edit.component.css'
})
export class FormEditComponent implements OnInit {
  formId = input.required<string>();

  private formService = inject(FormService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);

  private _isCreating = signal<boolean>(false);
  isCreating = this._isCreating.asReadonly();

  private _form = signal<FormUpdate | undefined>(undefined);
  form = this._form.asReadonly();

  selectedPreferenceIndex = signal<number | undefined>(undefined);
  formTitle = signal<string>('');
  isEditingTitle = signal<boolean>(false);

  ngOnInit(): void {
    this._isCreating.set(this.formId() === '');
    if (this.formId() === '') {
      this._form.set({
        id: -1,
        title: '',
        obligatoryPreferences: []
      });

      return;
    }

    this.formService.getFormContent(+this.formId()!).subscribe({
      next: response => {
        if (response.success) {
          const payload = response.payload!;
          this.formTitle.set(payload.title);
          this._form.set({
            id: payload.id,
            title: payload.title,
            obligatoryPreferences: payload.obligatoryPreferences.map(preference => {
              return {
                name: preference.name,
                options: preference.options.map(option => option.name)
              }
            })
          });
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    })
  }

  onTitleEditClick(): void {
    this.isEditingTitle.set(true);
  }

  onPreferenceEditClick(preferenceIndex: number): void {
    this.selectedPreferenceIndex.set(preferenceIndex);
  }

  onClose(): void {
    this.selectedPreferenceIndex.set(undefined);
  }

  onSave(preference: ObligatoryPreferenceCreate): void {
    this._form.update(data => {
      return {
        id: data!.id,
        title: this.formTitle(),
        obligatoryPreferences: data!.obligatoryPreferences.updateClone(this.selectedPreferenceIndex()!, preference)
      };
    });

    if (this.isCreating()) {
      return;
    }

    this.formService.updateForm(this.form()!).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Zapisano zmiany w ankiecie');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onSaveTitle(): void {
    this.isEditingTitle.set(false);
  }

  onAddPreference(): void {
    this._form.update(data => {
      this.selectedPreferenceIndex.set(data?.obligatoryPreferences.length);
      return {
        id: data!.id,
        title: this.formTitle(),
        obligatoryPreferences: [...data!.obligatoryPreferences, {
          name: '',
          options: []
        }]
      };
    });
  }

  onCreate(): void {
    this.formService.createForm({
      title: this.formTitle(),
      obligatoryPreferences: this.form()!.obligatoryPreferences
    }).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie utworzono ankietę');
          this.router.navigate(['admin', 'forms']);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onCancel(): void {
    this.toastrService.info('Tworznie ankiety anulowane - Nie utworzono ankiety');
    this.router.navigate(['admin', 'forms']);
  }
}
