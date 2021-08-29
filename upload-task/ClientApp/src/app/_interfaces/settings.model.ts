export interface Settings {
  _id: number
  min_file_size: number
  max_file_size: number
  destination_path: string
  allow_mimes: string
  allow_rename: boolean
  allow_override: boolean
}
