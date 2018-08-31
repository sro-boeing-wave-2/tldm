import { Component, OnInit } from '@angular/core';
import { ApplicationDataService } from '../application-data.service';
import { Application } from '../application';
import { Router } from '@angular/router';

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.css']
})
export class ApplicationsComponent implements OnInit {

  applications: Application []

  constructor(private _applicationdataservice: ApplicationDataService, private router: Router) { }

  ngOnInit() {
    this._applicationdataservice.getAll().subscribe(data => this.applications = data, err => console.log(err));
  }

  getDetails(id) {
    this.router.navigate(['/details', id]);
  }



}
