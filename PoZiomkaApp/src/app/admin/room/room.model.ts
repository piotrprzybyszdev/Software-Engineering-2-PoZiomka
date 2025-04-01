export type RoomModel = {
  id: number,
  floor: number,
  number: number,
  capacity: number,
  studentIds: number[],
  isFull: boolean
}

export type RoomCreate = {
  id: number,
  number: number,
  capacity: number
}
