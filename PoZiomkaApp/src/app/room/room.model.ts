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
