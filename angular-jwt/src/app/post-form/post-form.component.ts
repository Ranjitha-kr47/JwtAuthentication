import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Department } from '../Models/department';
import { Designation } from '../Models/designation';
import { Employee } from '../Models/employee';
import { DepartmentService } from '../service/department.service';
import { EmployeeService } from '../service/employee.service';
import { ActivatedRoute, Router } from '@angular/router';
//import { FormBuilder, FormControl, NumberValueAccessor, Validators } from '@angular/forms';


@Component({
  selector: 'app-post-form',
  templateUrl: './post-form.component.html',
  styleUrls: ['./post-form.component.css']
})
export class PostFormComponent implements OnInit {

  //departments=['Admin','Finance','IT'];
  //designations=['Associate', 'Executive' , 'Manager'];

  formGroup: FormGroup;
  user: Employee = new Employee;
 
  departments: Department[]=[];
  designations : Designation[]=[];
  
 
  constructor(private userService: EmployeeService,
    private departmentService: DepartmentService,
    //private fb: FormBuilder,
    private route: ActivatedRoute,private router:Router) { }
 

  //startDate = new Date(2000, 0, 2);
  

  

  ngOnInit(): void {
    this.user=new Employee();
    console.log(this.user)
    // this.employee = { 
    //   FirstName:"",
    //   LastName:"",
    //   Age: null,
    //   DateOfJoining:"",
    //   Department:"",
    // DepartmentId:null,
    // Designation:"",
    // DesignationId:null
    // };

    this.getDepartment();
    this.getDesignations(this.route.snapshot.paramMap.get('id'));
   
 
  }
  getDepartment(){
    this.departmentService.getDepartments().subscribe(data => {
      this.departments = data;
      console.log(data);
      console.log(this.departments)
    },
    error => {
      console.log(error);
    });
  }
  getDesignations(theDepartmentId){
    console.log(theDepartmentId);
    // let dep=1;
    // this.departments.forEach(department=>{if(department.departmentName===theDepartmentId){
    //   dep=department.id;
    // }})
    if (theDepartmentId === null || theDepartmentId === undefined){
      theDepartmentId = 1;
    }
    //console.log(dep);
    this.departmentService.getDesignations(theDepartmentId).subscribe( data => {
      this.designations = data;
      console.log(data);
    },
    error => {
      console.log(error);
    });
  }

  onSubmit() {
    console.log(this.user);
    this.saveUser();
  }

  onChange() {
    this.departments.forEach(department => {
      if(department.departmentName === this.user.Department) {
        this.user.DepartmentId = department.id;
      }
    });
    this.designations.forEach(designation => {
      if(this.user.DepartmentId === designation.departmentId) {
        this.user.Designation = designation.designationName;
        this.user.DesignationId = designation.designationId;
      }
    });
  }
  
  saveUser() {
    //this.employee.DepartmentId=this.departments.filter(value=>value['departmentName']===this.employee.Department)[0].DepartmentId;
    //delete this.employee.Department;
    let finalUser=this.user;
    finalUser.DepartmentId=Number(finalUser.DepartmentId);
    console.log(finalUser)
    console.log(this.designations)
    this.designations.forEach(designation=>{if(designation.departmentId==finalUser.DepartmentId){
      //console.log('hi')
      finalUser.DesignationId=designation.designationId;

    }})
    console.log(finalUser);
    finalUser['DesignationName']=finalUser.Designation;
    delete finalUser.Designation;
    this.userService.createUser(finalUser).subscribe(data => {
      console.log(data);
      //alert("User added successfully");
      this.router.navigate(['/login']);
    },
      error => console.log(error));
  }
}
  

  // gotoUserList() {
  //   //this.router.navigate(['/users']);
  // }

  


