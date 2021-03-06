import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CarsDataComponent } from './cars-list/cars-list.component';
import { MakersDataComponent } from './makers-list/makers-list.component';
import { ModelsDataComponent } from "./models-list/models-list.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CarsDataComponent,
    MakersDataComponent,
    ModelsDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'cars-list', component: CarsDataComponent },
      { path: 'makers-list', component: MakersDataComponent },
      { path: 'models-list', component: ModelsDataComponent }
])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
