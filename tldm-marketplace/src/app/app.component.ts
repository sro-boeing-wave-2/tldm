import { Component } from '@angular/core';
import { ApplicationDataService } from './application-data.service';
import { Application } from './application'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ApplicationDataService]

})
export class AppComponent {

  constructor(private applicationDataService: ApplicationDataService) {
  }

  onAddApplication(application) {

  }


}
