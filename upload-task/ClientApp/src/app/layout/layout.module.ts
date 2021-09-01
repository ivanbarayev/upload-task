import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {IndexComponent} from './index/index.component';
import {NavBarComponent} from './nav-bar/nav-bar.component';
import {RouterModule, Routes} from "@angular/router";

const routes: Routes = [
  {
    path     : '',
    component: IndexComponent
  }
];

@NgModule({
  declarations: [
    IndexComponent,
    NavBarComponent
  ],
  exports     : [
    NavBarComponent
  ],
  imports     : [
    CommonModule,
    RouterModule.forChild(routes),
  ]
})
export class LayoutModule {
}
