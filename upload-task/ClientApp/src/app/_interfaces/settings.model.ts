

export interface Responser {
  status: number,
  message: string,

}

export interface ResponseTaker {
  status: number,
  message: string,
  data : Settings
}

export interface Settings {
  _id: number
  min_file_size: number
  max_file_size: number
  destination_path: string
  allow_mimes: string
  store_type: string
  allow_rename: boolean
  allow_override: boolean
}
