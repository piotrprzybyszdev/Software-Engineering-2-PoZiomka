import { StudentModel } from "../student/student.model"

export type RoomModel = {
  id: number,
  floor: number,
  number: number,
  capacity: number,
  students: StudentModel[],
  isFull: boolean
}

export type RoomCreate = {
  id: number,
  number: number,
  capacity: number
}
