import { Component, Input, Output, EventEmitter, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReservationService } from '../../../reservation/reservation.service';
import { ReservationStudentModel } from '../../../reservation/reservation.model';
import { RoomService } from '../../../admin/rooms/room.service';
import { RoomStudentModel, roomStatusToString } from '../../../admin/rooms/room.model';
import { PopupComponent } from '../../../common/popup/popup.component';
import { StudentService } from '../../student.service';

@Component({
  selector: 'app-reservation-popup',
  standalone: true,
  imports: [CommonModule, PopupComponent],
  templateUrl: './room-reservation.component.html',
  styleUrl: './room-reservation.component.css'
})
export class ReservationPopupComponent implements OnInit {
  private reservationService = inject(ReservationService);
  private roomService = inject(RoomService);

  @Input() reservationId?: number;
  @Input() roomId?: number;

  @Output() hide = new EventEmitter<void>();

  reservation = signal<ReservationStudentModel | null>(null);
  room = signal<RoomStudentModel | null>(null);

  private studentService = inject(StudentService);
  loggedInStudent = this.studentService.loggedInStudent;

  ngOnInit(): void {
    if (this.reservationId) {
      this.reservationService.getReservation(this.reservationId).subscribe({
        next: res => {
          if (res.success) this.reservation.set(res.payload!);
        }
      });
    } else if (this.roomId) {
      this.roomService.getRoom(this.roomId).subscribe({
        next: res => {
          if (res.success) this.room.set(res.payload!);
        }
      });
    }
  }

  onHide(): void {
    this.hide.emit();
  }

  roomStatusToString = roomStatusToString;
}
