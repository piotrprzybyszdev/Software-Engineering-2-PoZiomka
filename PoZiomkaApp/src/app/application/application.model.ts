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
  applicationStatus: ApplicationStatus
}

export type ApplicationSearchParams = {
  studentEmail?: string,
  studentIndex?: string,
  applicationTypeId?: number,
  applicationStatus?: ApplicationStatus
}

export function saveFileToDisk(blob: Blob, fileName: string): void {
  const link = document.createElement('a');
  link.href = URL.createObjectURL(blob);
  link.download = fileName;
  link.click();
  link.remove();
}
