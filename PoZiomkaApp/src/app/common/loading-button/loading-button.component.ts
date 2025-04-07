import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-loading-button',
  imports: [CommonModule],
  templateUrl: './loading-button.component.html',
  styleUrl: './loading-button.component.css'
})
export class LoadingButtonComponent {
  isLoading = input.required<boolean>();
  isDisabled = input<boolean>();
  class = input<string>();

  clicked = output<void>();

  onClick(): void {
    this.clicked.emit();
  }
}
