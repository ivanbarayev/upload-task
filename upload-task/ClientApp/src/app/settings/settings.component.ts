import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Settings } from '../_interfaces/settings.model';
import { FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  public settings: Settings
  frm_settings: FormGroup
  is_valid: boolean = false

  constructor(private fb: FormBuilder, private http: HttpClient) {

  }

  ngOnInit() {
    this.http.get<Settings>('https://localhost:44367/api/settings').subscribe(result => {
      this.settings = result['data'];
    }, error => console.error(error));
  }

  saveSettings(f: NgForm) {
    console.log(f.value.destination_path)
    console.log(f.valid)
    this.is_valid = f.valid
    if (f.valid == true) {
      this.http.post('https://localhost:44367/api/settings', f.value, { observe: "events" })
        .subscribe(event => {
          if (event.type === HttpEventType.Response) {
            this.alerta(event.body.status, event.body.message)
          }
        });
    } else {
      this.alerta(2, "Please fill all the required fields")
    }
  }

  onSubmit2() {
    const frm_data = new FormData();
    frm_data.append('destination_path', this.frm_settings.get('destination_path').value);
    this.http.post('https://localhost:44367/api/settings', frm_data).subscribe(status => console.log(JSON.stringify(status)));
  }

  alerta(status: number, message: string) {
    Swal.fire({
      text: message,
      icon: (status == 1 ? 'success' : 'error'),
      confirmButtonText: 'Ok'
    })
  }
}
