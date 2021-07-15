import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { EmployeeService } from "./service/employee.service";

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelperService, private router: Router) {
  }
  canActivate() {
    const token = localStorage.getItem("jwt");

    if (token && !this.jwtHelper.isTokenExpired(token)){
        this.jwtHelper.decodeToken(token)
        //console.log(this.jwtHelper.decodeToken(token));
      return true;
    }
    this.router.navigate(["login"]);
    return false;
  }

// constructor(private employeeService: EmployeeService, private router: Router) {}

// canActivate(
//     route: ActivatedRouteSnapshot,
//     state: RouterStateSnapshot): boolean {
//     const url: string = state.url;
//     return this.checkLogin(url);
//   }

//   canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
//     return this.canActivate(route, state);
//   }

//   checkLogin(url: string) {
//     if (this.employeeService.isLoggedIn()) {
//       return true;
//     }

//     this.employeeService.redirectUrl = url;

//     this.router.navigate(['/login'], {queryParams: { returnUrl: url }} );
//   }

}
