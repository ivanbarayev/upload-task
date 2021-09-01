import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {SettingsComponent} from './settings/settings.component';
import {LayoutModule} from "./layout/layout.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent,
    SettingsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    LayoutModule,
    FormsModule
  ],
  providers   : [],
  bootstrap   : [AppComponent],
  exports: [ReactiveFormsModule]
})

export class AppModule {
}
