export type FormModel = {
  id: number,
  title: string
}

export type ObligatoryPreferenceOption = {
  id: number,
  name: string
}

export type ObligatoryPreferenceModel = {
  id: number,
  name: string,
  options: ObligatoryPreferenceOption[]
}

export type FormContentModel = {
  id: number,
  title: string,
  obligatoryPreferences: ObligatoryPreferenceModel[]
}

export type ObligatoryPreferenceCreate = {
  name: string,
  options: string[]
}

export type FormCreate = {
  title: string,
  obligatoryPreferences: ObligatoryPreferenceCreate[]
}

export type FormUpdate = {
  id: number,
  title: string,
  obligatoryPreferences: ObligatoryPreferenceCreate[]
}
