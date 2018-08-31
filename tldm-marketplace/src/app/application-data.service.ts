import { Injectable } from '@angular/core';
import { Application } from './application';
import { Http, Response } from '@angular/http';
import { map, catchError } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

const API_URL = environment.apiUrl;


@Injectable()

export class ApplicationDataService {

  constructor(private http: Http) { }

  getAll() {
    return this.http.get(API_URL + "/applications")
      .pipe(map(response => {
        const applications = response.json();
        return applications.map((application) => new Application(application));
      }))
      .pipe(catchError(this.handleError));
  }

  getById(id: number) {
    return this.http.get(API_URL + `/applications/${id}`)
      .pipe(map(response => {
        return new Application(response.json());
      }))
      .pipe(catchError(this.handleError));
  }

  public addApplication(application: Application): Observable<Application> {
    console.log(application);
    return this.http
      .post(API_URL + '/applications', application)
      .pipe(map(response => {
        return new Application(response.json());
      }))
      .pipe(catchError(this.handleError));
  }

  private handleError(error: Response | any) {
    console.error('ApiService::handleError', error);
    return Observable.throw(error);
  }
}
