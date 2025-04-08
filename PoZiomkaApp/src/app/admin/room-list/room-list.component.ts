import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RoomService } from '../../room/room.service';
import { RoomModel, RoomStatus } from '../../room/room.model';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-room-list',
  imports: [CommonModule],
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.css'
})
export class RoomListComponent implements OnInit {
  private roomService = inject(RoomService);
  private toastrService = inject(ToastrService);

  private _rooms = signal<RoomModel[]>([]);
  roomsPerFloor = computed(() => {
    const map = new Map<number, RoomModel[]>();
    for (const room of this._rooms()) {
      const current = map.get(room.floor);
      map.set(room.floor, [...current || [], room]);
    }
    return map;
  });

  getColorString(status: RoomStatus): string {
    switch (status) {
      case RoomStatus.Available:
        return "success";
      case RoomStatus.Reserved:
        return "warning";
      case RoomStatus.Occupied:
        return "info";
      case RoomStatus.Full:
        return "danger";
    }
  }

  getStatusString(status: RoomStatus): string {
    switch (status) {
      case RoomStatus.Available:
        return "Dostępny";
      case RoomStatus.Reserved:
        return "Zarezerwowany";
      case RoomStatus.Occupied:
        return "Zajęty";
      case RoomStatus.Full:
        return "Pełny";
    }
  }

  ngOnInit(): void {
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
