import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { PopupComponent } from "../../../../common/popup/popup.component";
import { ObligatoryPreferenceCreate } from '../../form.model';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-preference-edit',
  imports: [PopupComponent, ReactiveFormsModule],
  templateUrl: './preference-edit.component.html',
  styleUrl: './preference-edit.component.css'
})
export class PreferenceEditComponent implements OnInit {
  originalPreference = input.required<ObligatoryPreferenceCreate>();
  private _preference = signal<ObligatoryPreferenceCreate | undefined>(undefined);
  preference = this._preference.asReadonly();

  close = output<void>();
  save = output<ObligatoryPreferenceCreate>();

  formBuilder = inject(FormBuilder);

  isEditing = signal<boolean>(false);

  form = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    options: this.formBuilder.array<string>([])
  });

  get options() {
    return this.form.controls['options'] as FormArray;
  }

  ngOnInit(): void {
    this.isEditing.set(this.originalPreference().name === '');
    this._preference.set(this.originalPreference());
    this.form.controls.name.patchValue(this.originalPreference().name);
    this.originalPreference().options.forEach(option => {
      this.options.push(this.formBuilder.control(option, [Validators.required, Validators.maxLength(100)]))
    });
  }

  onClose(): void {
    this.close.emit();
  }

  onEditClick(): void {
    this.isEditing.set(true);
  }

  onSaveClick(): void {
    this.isEditing.set(false);

    const newPreference = {
      name: this.form.controls.name.value ?? '',
      options: this.options.controls.map(ctrl => ctrl.value)
    };

    this._preference.set(newPreference);
    this.save.emit(newPreference);
  }

  onAddOptionClick(): void {
    this.options.push(this.formBuilder.control('', [Validators.required, Validators.maxLength(100)]));
  }

  onRemoveOptionClick(index: number): void {
    this.options.removeAt(index);
  }
}
