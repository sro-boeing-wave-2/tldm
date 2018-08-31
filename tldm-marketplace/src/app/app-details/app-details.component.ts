import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ApplicationDataService } from '../application-data.service';
import { Application } from '../application';

@Component({
  selector: 'app-app-details',
  templateUrl: './app-details.component.html',
  styleUrls: ['./app-details.component.css']
})
export class AppDetailsComponent implements OnInit {
  selectedId;
  application;

  constructor(private activatedroute: ActivatedRoute, private applicationdataservice: ApplicationDataService, private router: Router) { }

  ngOnInit() {
    this.activatedroute.paramMap.subscribe((params: ParamMap) => {
      this.selectedId = parseInt(params.get('id'));
    });
    this.applicationdataservice.getById(this.selectedId).subscribe(data => this.application=data);
  }

}
