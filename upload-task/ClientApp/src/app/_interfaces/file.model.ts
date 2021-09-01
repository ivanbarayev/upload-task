export interface ResponseTaker {
  status: number,
  message: string,
  data: any
}

export interface FileInf {
  FileName: string,
  FileMime: string,
  FileSize: string,
  FilePathSrc: string,
  FilePathDest: string,
  UploadDate: Date
}

export interface FileView {
  store_type: string,
  file_name_org: string,
  file_name_gen: string,
  image: string
}
