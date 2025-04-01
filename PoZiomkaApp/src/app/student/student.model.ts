export type StudentModel = {
  id: number,
  email: string,
  firstName?: string,
  lastName?: string,
  indexNumber?: string,
  phoneNumber?: string,
  reservationId?: number,
  hasAcceptedReservation?: boolean,
  canFillForms?: boolean
}

export type StudentCreate = {
  email: string,
  firstName?: string,
  lastName?: string,
  indexNumber?: string,
  phoneNumber?: string
}

export type StudentUpdate = {
  id: number;
  firstName?: string;   
  lastName?: string;    
  indexNumber?: string; 
  phoneNumber?: string; 
};