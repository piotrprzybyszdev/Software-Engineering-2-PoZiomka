import { Component, computed, effect, inject, OnInit, signal } from '@angular/core';
import { RoomService } from '../../room/room.service';
import { RoomModel, RoomStatus, RoomStudentModel } from '../../room/room.model';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { PopupComponent } from "../../common/popup/popup.component";
import { StudentService } from '../../student/student.service';
import { StudentModel } from '../../student/student.model';

@Component({
  selector: 'app-room-list',
  imports: [CommonModule, PopupComponent],
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css'
})
export class RoomListComponent implements OnInit {
  private roomService = inject(RoomService);
  private studentService = inject(StudentService);
  private toastrService = inject(ToastrService);

  private _students = signal<StudentModel[]>([]);
  private _rooms = signal<RoomModel[]>([]);
  roomsPerFloor = computed(() => {
    const map = new Map<number, RoomModel[]>();
    for (const room of this._rooms()) {
      const current = map.get(room.floor);
      map.set(room.floor, [...current || [], room]);
    }
    return map;
  });

  isAddingRoom = signal<boolean>(false);
  selectedRoomId = signal<number | undefined>(undefined);
  private _selectedRoom = signal<RoomStudentModel | undefined>(undefined);
  selectedRoom = this._selectedRoom.asReadonly();

  otherStudents = computed(() => this._students().filter(otherStudent => !this.selectedRoom()?.students.find(student => student.id === otherStudent.id)));

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

  constructor() {
    effect(() => {
      const id = this.selectedRoomId();

      if (id === undefined) {
        this._selectedRoom.set(undefined);
        return;
      }

      this.roomService.getRoom(id).subscribe({
        next: response => {
          if (response.success) {
            this._selectedRoom.set(response.payload!);
          } else {
            this.toastrService.error(response.error!.detail, response.error!.title);
          }
        }
      });
    });
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

    this.studentService.getAllStudents().subscribe({
      next: response => {
        if (response.success) {
          this._students.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onAddRoomClick(): void {
    this.isAddingRoom.set(true);
  }

  onAddRoomCancelClick(): void {
    this.isAddingRoom.set(false);
  }

  onRoomClick(room: RoomModel): void {
    this.selectedRoomId.set(room.id);
  }

  onRoomDeselect(): void {
    this.selectedRoomId.set(undefined);
  }

  onRoomDelete(): void {
    this.roomService.deleteRoom(this.selectedRoom()!.id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie usunięto pokój');
          this.onRoomDeselect();
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onStudentRemove(studentId: number): void {
  }

  onStudentAdd(studentId: number): void {
  }
}
