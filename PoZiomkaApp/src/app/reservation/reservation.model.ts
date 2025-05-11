import { RoomModel } from "../admin/rooms/room.model";
import { StudentModel } from "../student/student.model";

export type ReservationModel = {
  id: number,
  roomId: number,
  isAcceptedByAdmin: boolean
};

export type ReservationStudentModel = {
  id: number,
  room: RoomModel,
  students: StudentModel[],
  isAcceptedByAdmin: boolean
};
