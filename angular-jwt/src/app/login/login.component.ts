import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Login } from '../Models/login';
import { EmployeeService } from '../service/employee.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit  {

  private subscription: Subscription;
  invalidLogin: boolean;

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  //credentials: Login = { Email: '', Password: '' };
  //credentials:Login=new Login;
  activatedRoute: any;
  constructor(private employeeService:EmployeeService,private router:Router, private httpClient:HttpClient) { }

  ngOnInit(): void {
    // this.subscription = this.activatedRoute.queryParams.subscribe(
    //   (param: any) => {
    //      this.brandNew = param['brandNew'];   
    //      this.credentials.Email = param['email'];         
    //   }); 
  }

  // ngOnDestroy() {
  //   // prevent memory leak by unsubscribing
  //   this.subscription.unsubscribe();
  // }

  // login({ value, valid }: { value: Login, valid: boolean }) {
  //   this.submitted = true;
  //   this.isRequesting = true;
  //   this.errors='';
  //   if (valid) {
  //     this.employeeService.login(value.Email, value.Password)
  //       .finally(() => this.isRequesting = false)
  //       .subscribe(
  //       result => {         
  //         if (result) {
  //            this.router.navigate(['/home']);             
  //         }
  //       },
  //       error => this.errors = error);
  //   }
  // }

  login(form: NgForm) {
    this.employeeService.login(form);
    
  }

}
