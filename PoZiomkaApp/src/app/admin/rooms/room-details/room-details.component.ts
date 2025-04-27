import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { PopupComponent } from '../../../common/popup/popup.component';
import { roomStatusToColorString, roomStatusToString, RoomStudentModel } from '../room.model';
import { CommonModule } from '@angular/common';
import { LoadingButtonComponent } from '../../../common/loading-button/loading-button.component';
import { StudentModel } from '../../../student/student.model';
import { FormsModule } from '@angular/forms';
import { RoomService } from '../room.service';
import { ToastrService } from 'ngx-toastr';
import { StudentService } from '../../../student/student.service';

@Component({
  selector: 'app-room-details',
  imports: [CommonModule, FormsModule, PopupComponent, LoadingButtonComponent],
  templateUrl: './room-details.component.html',
  styleUrl: './room-details.component.css'
})
export class RoomDetailsComponent {
  hide = output<void>();
  roomId = input.required<number>();

  students = signal<StudentModel[]>([]);
  room = signal<RoomStudentModel | undefined>(undefined);

  isRemoving = signal<boolean[]>([]);
  isAdding = signal<boolean[]>([]);
  isAddingOrRemoving = computed(() => !this.isAdding().every(b => b === false) || !this.isRemoving().every(b => b === false));

  private studentService = inject(StudentService);
  private roomService = inject(RoomService);
  private toastrService = inject(ToastrService);

  otherStudents = computed(() => this.students()
    .filter(otherStudent => !this.room()?.students.find(student => student.id === otherStudent.id))
    .filter(otherStudent => !otherStudent.roomId)
    .filter(otherStudent => otherStudent.email.startsWith(this.email()) &&
    otherStudent.indexNumber?.startsWith(this.indexNumber()) &&
    otherStudent.phoneNumber?.startsWith(this.phoneNumber()) &&
    otherStudent.firstName?.startsWith(this.firstName()) &&
    otherStudent.lastName?.startsWith(this.lastName()))
  );

  roomStatusToString = roomStatusToString;
  roomStatusToColorString = roomStatusToColorString;

  firstName = signal<string>('');
  lastName = signal<string>('');
  email = signal<string>('');
  indexNumber = signal<string>('');
  phoneNumber = signal<string>('');

  isDeleteLoading = signal<boolean>(false);

  constructor() {
    effect(() => this.refreshRoomInfo());
    effect(() => this.isAdding.set(Array(this.otherStudents().length).fill(false)));
  }

  onHide(): void {
    if (this.isDeleteLoading()) {
      return;
    }

    this.hide.emit();
  }

  onRoomDelete(): void {
    if (this.room() === undefined) {
      return;
    }

    this.isDeleteLoading.set(true);
    this.roomService.deleteRoom(this.room()!.id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie usunięto pokój');
          this.isDeleteLoading.set(false);
          this.hide.emit();
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
          this.isDeleteLoading.set(false);
        }
      }
    });
  }

  onStudentRemove(studentIndex: number): void {
    if (this.room() === undefined) return;

    this.isRemoving.update(arr => arr.updateClone(studentIndex, true));
    this.roomService.removeStudentFromRoom(this.room()!.id, this.room()!.students[studentIndex].id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Usunięto studenta z pokoju');
          this.refreshRoomInfo(); 
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isRemoving.update(arr => arr.updateClone(studentIndex, false));
      }
    });
  }

  onStudentAdd(studentIndex: number): void {
    if (this.room() === undefined) return;

    this.isAdding.update(arr => arr.updateClone(studentIndex, true));
    this.roomService.addStudentToRoom(this.room()!.id, this.otherStudents()![studentIndex].id).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Dodano studenta do pokoju');
          this.refreshRoomInfo(); 
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isAdding.update(arr => arr.updateClone(studentIndex, false));
      }
    });
  }

  private refreshRoomInfo(): void {
    this.roomService.getRoom(this.roomId()).subscribe({
      next: response => {
        if (response.success) {
          this.room.set(response.payload);
          this.isRemoving.set(Array(response.payload!.students.length).fill(false));
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });

    this.studentService.getAllStudents().subscribe({
      next: response => {
        if (response.success) {
          this.students.set(response.payload!);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
}

