import {Injectable} from '@angular/core';
import Swal from "sweetalert2";

@Injectable({
  providedIn: 'root'
})
export class CommonsService {
  public API_URL: string = "http://localhost:45200/api/"
  public SUCCESS: number = 1
  public ERROR: number = 2

  constructor() {
  }

  alerta(status: number, message: string) {
    Swal.fire({
      text             : message,
      icon             : (status == 1 ? 'success' : 'error'),
      confirmButtonText: 'Ok'
    })
  }

  alertaIMG(base64Str: string) {
    Swal.fire({
      imageUrl: base64Str,
      imageWidth: 400,
      imageHeight: 350
    })
  }

  alertaConfirm() {
    return Swal.fire({
      title: 'Do you want to delete the file ?',
      showDenyButton: true,
      showCancelButton: true,
      confirmButtonText: `Yes`,
      denyButtonText: `Don't delete`,
    })
  }

  convertFileSize(unit_type: number, size: number) {
    let calculated_unit: string;
    if (unit_type == 1) {
      calculated_unit = (size / 1024).toFixed(2) + " KB";
    } else if (unit_type == 2) {
      calculated_unit = (size / 1048576).toFixed(2) + " MB";
    } else {
      calculated_unit = size + " Bytes";
    }
    return calculated_unit;
  }


}
