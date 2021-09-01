import { Injectable } from '@angular/core';
import {ResponseTaker, Settings} from "../_interfaces/settings.model";
import {FormBuilder, NgForm} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {CommonsService} from "./commons.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  is_valid: null | boolean = false
  public settings: Settings | undefined

  constructor(
    private http: HttpClient,
    private cm: CommonsService
  ) { }

  getSettings() : Observable<ResponseTaker>{
    return this.http.get<ResponseTaker>(this.cm.API_URL + 'settings').pipe();
  }

  saveSettings(f: NgForm) {
    this.is_valid = f.valid
    if (f.valid == true) {
      this.http.patch<ResponseTaker>(this.cm.API_URL + 'settings', f.value).subscribe(event => {
        this.cm.alerta(event.status, event.message)
      });
    } else {
      this.cm.alerta(this.cm.ERROR, "Please fill all the required fields")
    }
  }
}
