import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { ApplicationDataService } from './application-data.service';
import { AppDetailsComponent } from './app-details/app-details.component';
import { AddAppComponent } from './add-app/add-app.component';
import { AppRoutingModule } from './app-routing.module';
import { ApplicationsComponent } from './applications/applications.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import {MatIconModule} from '@angular/material/icon';
import { SelectChannelComponent } from './select-channel/select-channel.component';

@NgModule({
  declarations: [
    AppComponent,
    AppDetailsComponent,
    AddAppComponent,
    ApplicationsComponent,
    SelectChannelComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatGridListModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule
  ],
  providers: [ApplicationDataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
