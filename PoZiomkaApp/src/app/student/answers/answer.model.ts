import { FormModel, ObligatoryPreferenceModel } from "../../admin/forms/form.model"

export type ChoosableAnswerModel = {
  id: number,
  name: string,
  isHidden: boolean
}

export type ObligatoryAnswerModel = {
  id: number | null,
  obligatoryPreference: ObligatoryPreferenceModel,
  obligatoryPreferenceOptionId: number | null,
  isHidden: boolean
}

export type AnswerModel = {
  id: number | null,
  formId: number,
  studentId: number,
  status: FormStatus,
  choosableAnswers: ChoosableAnswerModel[],
  obligatoryAnswers: ObligatoryAnswerModel[]
}

export enum FormStatus {
  NotFilled, InProgress, Filled
}

export type AnswerStatus = {
  id: number | null,
  form: FormModel,
  status: FormStatus
}

export type ChoosableAnswerCreate = {
  name: string,
  isHidden: boolean
}

export type ObligatoryAnswerCreate = {
  obligatoryPreferenceId: number,
  obligatoryPreferenceOptionId: number,
  isHidden: boolean
}

export type AnswerCreate = {
  formId: number,
  choosableAnswers: ChoosableAnswerCreate[],
  obligatoryAnswers: ObligatoryAnswerCreate[]
}

export type AnswerUpdate = {
  formId: number,
  choosableAnswers: ChoosableAnswerCreate[],
  obligatoryAnswers: ObligatoryAnswerCreate[]
}
