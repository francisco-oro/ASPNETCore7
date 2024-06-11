import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CitiesComponent} from "./cities/cities.component";
import {AppComponent} from "./app.component";

const routes: Routes = [
  {path: 'cities', component: CitiesComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
