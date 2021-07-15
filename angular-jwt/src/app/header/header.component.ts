import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { EmployeeService } from '../service/employee.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private loginService:EmployeeService) { }
  invalidLogin=true;
  loggedIn$:Subscription;
  ngOnInit(): void {
    //this.invalidLogin=this.loginService.getInvalidLogin();
    this.loggedIn$=this.loginService.isLoggedIn.subscribe(
      value=>this.invalidLogin=!value
    );
  }

logout(){
  this.loginService.logout();
}

}
