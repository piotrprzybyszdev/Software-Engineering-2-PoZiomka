export enum ApplicationStatus {
  pending, accepted, rejected
}

export type ApplicationTypeModel = {
  id: number,
  name: string,
  description: string
}

export type ApplicationModel = {
  id: number,
  studentId: number,
  applicationType: ApplicationTypeModel,
  file: File
  applicationStatus: ApplicationStatus
}

export type ApplicationSearchParams = {
  studentId?: number,
  studentEmail?: string,
  studentIndex?: string,
  status?: ApplicationStatus
}
