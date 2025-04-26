import { Component, EventEmitter, Output } from '@angular/core';
import { RoomService } from '../room.service';
import { RoomCreate } from '../room.model';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PopupComponent } from "../../../common/popup/popup.component";

@Component({
  selector: 'app-room-add',
  imports: [CommonModule, FormsModule, PopupComponent],
  templateUrl: './room-add.component.html',
  styleUrls: ['./room-add.component.css']
})
export class RoomAddComponent {
  @Output() hide = new EventEmitter<void>();
  
  newRoomFloor: number = 0;
  newRoomNumber: number = 0;
  newRoomCapacity: number = 1;

  constructor(
    private roomService: RoomService,
    private toastrService: ToastrService
  ) {}

  onHide(): void {
    this.hide.emit();
  }

  onAddRoomConfirmClick(): void {
    const roomCreate: RoomCreate = {
      floor: this.newRoomFloor,
      number: this.newRoomNumber,
      capacity: this.newRoomCapacity
    };

    this.roomService.createRoom(roomCreate).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pokój został dodany');
          this.hide.emit();
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
}

