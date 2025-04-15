import { FormModel, ObligatoryPreferenceModel } from "../../form/form.model"

export type ChoosableAnswerModel = {
  id: number,
  name: string,
  isHidden: boolean
}

export type ObligatoryAnswerModel = {
  id: number,
  obligatoryPreference: ObligatoryPreferenceModel,
  obligatoryPreferenceOptionId: number,
  isHidden: boolean
}

export type AnswerModel = {
  id: number,
  formId: number,
  choosableAnswers: ChoosableAnswerModel[],
  ogligatoryAnswers: ObligatoryAnswerModel[]
}

export enum FormStatus {
  NotFilled, InProgress, Filled
}

export type AnswerStatus = {
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
  id: number,
  choosableAnsers: ChoosableAnswerCreate[],
  obligatoryAnswers: ObligatoryAnswerCreate[]
}
