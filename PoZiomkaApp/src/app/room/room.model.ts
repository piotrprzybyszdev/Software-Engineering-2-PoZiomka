import { StudentModel } from "../student/student.model"

export enum RoomStatus {
  Available, Reserved, Occupied, Full
}

export type RoomModel = {
  id: number,
  floor: number,
  number: number,
  capacity: number,
  reservationId?: number,
  studentIds: number[],
  status: RoomStatus
}

export type RoomStudentModel = {
  id: number,
  floor: number,
  number: number,
  capacity: number,
  students: StudentModel[],
  status: RoomStatus
}

export type RoomCreate = {
  id: number,
  number: number,
  capacity: number
}

export function roomStatusToString(status: RoomStatus): string {
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

export function roomStatusToColorString(status: RoomStatus): string {
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
