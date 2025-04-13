import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-popup',
  imports: [CommonModule],
  templateUrl: './popup.component.html',
  styleUrl: './popup.component.css'
})
export class PopupComponent {
  size = input.required<string>();

  outsideClicked = output<void>();
  closeClicked = output<void>();

  onOutsideClick(): void {
    this.outsideClicked.emit();
  }

  onInsideClick(event: Event): void {
    event.stopPropagation();
  }

  onCloseClick(): void {
    this.closeClicked.emit();
  }
}
