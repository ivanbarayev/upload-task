import {Component, OnInit} from '@angular/core';
import {ViewEncapsulation} from "@angular/core";
import {ResponseTaker, Settings} from "../_interfaces/settings.model";
import {FormBuilder, FormGroup, NgForm} from "@angular/forms";
import {HttpClient, HttpEventType} from "@angular/common/http";
import {CommonsService} from "../services/commons.service";
import {SettingsService} from "../services/settings.service";

@Component({
  selector     : 'app-settings',
  templateUrl  : './settings.component.html',
  styleUrls    : ['./settings.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class SettingsComponent implements OnInit {
  public settings: any
  is_valid: null | boolean = false

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private cm: CommonsService,
    private settings_service: SettingsService
  ) {
    this.settings_service.getSettings().subscribe(res => {
      this.settings = res.data;
    });
  }

  ngOnInit(): void {
  }

  saveSettings(f: NgForm) {
    this.settings_service.saveSettings(f)
  }
}
