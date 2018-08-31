import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Application} from '../application'
import { ApplicationDataService } from '../application-data.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-app',
  templateUrl: './add-app.component.html',
  styleUrls: ['./add-app.component.css'],
  providers: [ApplicationDataService]

})
export class AddAppComponent implements OnInit {

  newApplication: Application = new Application();

  applications: Application[] = [];


  @Output()
  add: EventEmitter<Application> = new EventEmitter()

  constructor(private applicationDataService: ApplicationDataService, private router: Router) { }

  ngOnInit() {
  }

  addApplication() {
    console.log("HI");
    this.applicationDataService
      .addApplication(this.newApplication)
      .subscribe(
        (newApplication) => {
          this.applications = this.applications.concat(newApplication);
        }
      );
    this.newApplication = new Application();
    this.router.navigate(['']);
  }

}
