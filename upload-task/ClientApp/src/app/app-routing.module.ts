import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SettingsComponent} from "./settings/settings.component";

const routes: Routes = [
  {path: 'dashboard', loadChildren: () => import('./layout/layout.module').then(m => m.LayoutModule)},
  {path: 'upload', loadChildren: () => import('./upload/upload.module').then(m => m.UploadModule)},
  {path: 'settings', component: SettingsComponent, pathMatch: 'full'},
  {path: '**', redirectTo : 'dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule {
}
