export enum MatchStatus {
  pending, accepted, rejected
};

export type MatchModel = {
  id: number,
  studentId1: number,
  studentId2: number,
  status1: MatchStatus,
  status2: MatchStatus
  isAccepted: boolean,
  isRejected: boolean
};
