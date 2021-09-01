import {Component, OnInit} from '@angular/core';
import {UploadService} from "../../services/upload.service";
import {ServerDataSource} from "ng2-smart-table";
import {HttpClient} from "@angular/common/http";
import {CommonsService} from "../../services/commons.service";

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})

export class ListComponent implements OnInit {
  dataList: any = [];
  settings = this.upload_service.settings

  constructor(private upload_service: UploadService, private http: HttpClient, private cm: CommonsService) {
    this.dataList = new ServerDataSource(http,
      {
        dataKey      : 'data.data',
        endPoint     : this.cm.API_URL + 'upload',
        pagerPageKey : 'page',
        pagerLimitKey: 'pageSize',
        totalKey     : 'data.total',
      });
  }

  viewUpload(event: any) {
    this.upload_service.viewUpload(event.data._id).subscribe(data => {
      this.cm.alertaIMG(data.data)
    })
  }

  ngOnInit(): void {

  }

  onDeleteConfirm(event : any) {
    this.cm.alertaConfirm().then((result) => {
      if (result.isConfirmed) {
        this.upload_service.deleteUpload(event.data.store_type, event.data._id)
        this.dataList.remove(event.data);
      }
    })
  }
}
