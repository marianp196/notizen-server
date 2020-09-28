import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { NoteListComponent } from './note-list/note-list.component';
import { HttpClientModule } from '@angular/common/http';
import { NoteListItemComponent } from './note-list-item/note-list-item.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NoteEditComponent } from './note-edit/note-edit.component';
import { SearchMachineComponent } from './search-machine/search-machine.component';

@NgModule({
  declarations: [
    AppComponent,
    NoteListComponent,
    NoteEditComponent,
    NoteListItemComponent,
    SearchMachineComponent
  ],
  entryComponents: [
    NoteEditComponent
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
