export enum RoomStatus {
  Available, Reserved, Occupied, Full
}

export type RoomModel = {
  id: number,
  floor: number,
  number: number,
  capacity: number,
  studentIds: number[],
  status: RoomStatus
}

export type RoomCreate = {
  id: number,
  number: number,
  capacity: number
}
