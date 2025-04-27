import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RoomService } from './room.service';
import { roomStatusToColorString, roomStatusToString, RoomModel, RoomStatus } from './room.model';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { RoomDetailsComponent } from './room-details/room-details.component';
import { RoomAddComponent } from "./room-add/room-add.component";
import { FormsModule } from '@angular/forms';
import { EnumPipe } from "../../common/enum-pipe";

@Component({
  selector: 'app-room-list',
  imports: [CommonModule, FormsModule, RoomDetailsComponent, RoomAddComponent, EnumPipe],
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css'
})
export class RoomListComponent implements OnInit {
  private roomService = inject(RoomService);
  private toastrService = inject(ToastrService);

  private _rooms = signal<RoomModel[]>([]);
  rooms = computed(() => this._rooms()
    .filter(room => room.number?.toString().startsWith(this.roomNumber()))
    .filter(room => room.floor.toString().startsWith(this.roomFloor()))
    .filter(room => this.roomStatus() === undefined || room.status === this.roomStatus())
  );

  isAddingRoom = signal<boolean>(false);
  selectedRoomId = signal<number | undefined>(undefined);

  RoomStatus = RoomStatus;
  roomStatusToString = roomStatusToString;
  roomStatusToColorString = roomStatusToColorString;

  roomNumber = signal<string>('');
  roomFloor = signal<string>('');
  roomStatus = signal<RoomStatus | undefined>(undefined);

  ngOnInit(): void {
    this.refreshRoomsAndStudents();
  }

  onAddRoomClick(): void {
    this.isAddingRoom.set(true);
  }

  onAddRoomCancelClick(): void {
    this.isAddingRoom.set(false);
    this.refreshRoomsAndStudents();
  }

  onRoomClick(room: RoomModel): void {
    this.selectedRoomId.set(room.id);
  }

  onRoomDeselect(): void {
    this.selectedRoomId.set(undefined);
    this.refreshRoomsAndStudents();
  }

  refreshRoomsAndStudents(): void {
    this.roomService.getRooms().subscribe({
      next: response => {
        if (response.success) {
          this._rooms.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
}
