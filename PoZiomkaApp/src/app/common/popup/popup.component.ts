import { Component, output } from '@angular/core';

@Component({
  selector: 'app-popup',
  imports: [],
  templateUrl: './popup.component.html',
  styleUrl: './popup.component.css'
})
export class PopupComponent {
  outsideClicked = output<void>();

  onOutsideClick(): void {
    this.outsideClicked.emit();
  }

  onInsideClick(event: Event): void {
    event.stopPropagation();
  }
}
