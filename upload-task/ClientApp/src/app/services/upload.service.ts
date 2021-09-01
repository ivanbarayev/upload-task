import {Injectable} from '@angular/core';
import {ResponseTaker} from "../_interfaces/file.model";
import {FormBuilder} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {CommonsService} from "./commons.service";

@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private cm: CommonsService
  ) {
  }

  public settings = {
    editable     : false,
    search       : false,
    mode         : "inline",
    type         : 'text',
    hideSubHeader: true,
    delete       : {
      confirmDelete: true
    },
    actions      : {
      add   : false,
      edit  : false,
      delete: true
    },
    attr         : {
      class: 'table table-striped table-bordered table-hover table-responsive'
    },
    pager        : {
      display: true
    },
    columns      : {
      _id          : {
        hide: true
      },
      store_type   : {
        title: 'Store Type'
      },
      file_mime    : {
        title: 'MIME'
      },
      file_name: {
        title: 'File Name'
      },
      file_size    : {
        title: 'File Size(B)'
      },
      upload_date  : {
        title: 'Date'
      }
    }
  };

  saveUpload(form_data: FormData) {
    return this.http.post<ResponseTaker>(this.cm.API_URL + 'upload', form_data).pipe()
  }

  listUpload() {
    return this.http.get<ResponseTaker>(this.cm.API_URL + 'upload').pipe()
  }

  viewUpload(id: string) {
    return this.http.get<ResponseTaker>(this.cm.API_URL + 'upload/' + id + '').pipe()
  }

  deleteUpload(store_type: string, id: string) {
    return this.http.delete<ResponseTaker>(this.cm.API_URL + 'upload/' + store_type + '/' + id + '').subscribe(event => {
      this.cm.alerta(event.status, event.message)
    })
  }
}
