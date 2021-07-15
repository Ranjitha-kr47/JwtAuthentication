import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { config } from 'rxjs/internal/config';
import { Employee } from '../Models/employee';
import { Observable } from 'rxjs';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { throwError } from 'rxjs/internal/observable/throwError';
import 'rxjs/add/operator/map';

import 'rxjs/Rx';
import { catchError, map } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  apiUrl="https://localhost:5001/api/authentication";

  
  //private invalidLogin = false;
  isLoggedIn=new BehaviorSubject(false);
  errorData: {};

  
  constructor(private httpClient:HttpClient, private router:Router) { 
    //super();
    //this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    //this._authNavStatusSource.next(this.loggedIn);
    //this.baseUrl = "https://localhost:5001/api";
  }

  createUser(employee: Employee) {
    return this.httpClient.post(`${this.apiUrl}`,employee);
  }

  // register(employee:Employee): Observable<Employee[]> {
  //   let body = JSON.stringify({ employee });
  //   let headers = new Headers({ 'Content-Type': 'application/json' });
  //   let options = new RequestOptions({ headers: headers });

  //   return this.httpClient.post(this.baseUrl + "/accounts", body,options)
  //     .map(res => true)
  //     .catch(this.handleError);
  // }  

  login(form: NgForm) {
    const credentials = JSON.stringify(form.value);
    
    this.httpClient.post("https://localhost:5001/api/authentication/login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      console.log(response);
      const token = (<any>response).token;
      localStorage.setItem("jwt", token);
      this.isLoggedIn.next(true);
      this.router.navigate(["/home"]);
    }, err => {
      console.log(err);
      this.isLoggedIn.next(false);
    });
    
  }

  logout(){
    localStorage.removeItem("jwt");
    this.isLoggedIn.next(false);
    this.router.navigate(['/login'])
  }

  // getInvalidLogin(){
  //   return this.invalidLogin;
  // }

}


