import { Component, inject, OnInit, signal } from '@angular/core';
import { RoomService } from '../room.service';
import { RoomModel } from '../room.model';
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
  rooms = this._rooms.asReadonly();

  getColorString(capacity: number, studentCount: number): string {
    if (capacity === studentCount) {
      return "danger";
    }
    if (studentCount === 0) {
      return "success";
    }
    return "warning";
  }

  getStatusString(capacity: number, studentCount: number): string {
    if (capacity === studentCount) {
      return "Niedostępny";
    }
    if (studentCount === 0) {
      return "Dostępny";
    }
    return "Zarezerwowany";
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
