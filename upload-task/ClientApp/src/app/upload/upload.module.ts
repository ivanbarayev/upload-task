import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormComponent} from './form/form.component';
import {ListComponent} from './list/list.component';
import {RouterModule, Routes} from "@angular/router";
import {FormBuilder, FormsModule} from "@angular/forms";
import { Ng2SmartTableModule } from 'ng2-smart-table';

const routes: Routes = [
  {
    path     : 'form',
    component: FormComponent
  },
  {
    path     : 'list',
    component: ListComponent
  }
];

@NgModule({
  declarations: [
    ListComponent,
    FormComponent
  ],
  imports     : [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    Ng2SmartTableModule
  ]
})

export class UploadModule {
}
