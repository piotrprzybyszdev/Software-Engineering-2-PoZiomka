import { Component, output } from '@angular/core';
import { PopupComponent } from '../../../common/popup/popup.component';

@Component({
  selector: 'app-room-add',
  imports: [PopupComponent],
  templateUrl: './room-add.component.html',
  styleUrl: './room-add.component.css'
})
export class RoomAddComponent {
  hide = output<void>();

  onHide(): void {
    this.hide.emit();
  }
}
