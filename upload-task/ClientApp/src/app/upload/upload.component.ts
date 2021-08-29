import { HttpClient, HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { FileInf } from '../_interfaces/file.models';


@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

  public file_inf: FileInf;
  public imgURL: any;
  public message: string;
  frm_upload: FormGroup;
  file: File = null;
  public upload_progress: number;
  upload_sub: Subscription;
  constructor(private fb: FormBuilder, private http: HttpClient) { }


  ngOnInit() {
    this.imgURL = "data:image/png;base64,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"
  }

  preview(files) {
    if (files.length === 0) {
      alert()
      return;
    }
    this.file = files[0];
    var mimeType = files[0].type;
    console.log(mimeType)
    if (mimeType.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }

    var reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    }
    this.file_inf = {
      FileName: files[0].name,
      FileMime: files[0].type,
      FileSize: this.convertFileSize(1, files[0].size),
      FilePathSrc: this.convertFileSize(2, files[0].size),
      FilePathDest: this.convertFileSize(0, files[0].size),
      UploadDate: new Date(),
    }
  }

  saveUpload() {
    //console.log(this.file);
    if (this.file == null) {
      this.alerta(2, "Please Select a file !")
      return
    } else {
      const form_data = new FormData();
      form_data.append('files', this.file);
      this.upload_progress = 10
      this.http.post('https://localhost:44367/api/upload', form_data, { reportProgress: true, observe: "events" })
        .subscribe(event => {
          if (event.type === HttpEventType.UploadProgress) {
            this.upload_progress = Math.round(100 * event.loaded / event.total);
          } else if (event.type === HttpEventType.Response) {
            this.alerta(event.body.status, event.body.message)
            this.upload_progress = 0
          }          
        });
    }
  }

  reset() {
    this.upload_progress = null;
    this.upload_sub = null;

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

  alerta(status: number, message: string) {
    Swal.fire({
      text: message,
      icon: (status == 1 ? 'success' : 'error'),
      confirmButtonText: 'Ok'
    })
  }

}
