Q1:

===
 
employee.model.ts

------------------

export class ContactDetails

{

    address : string ="";

    phone : string = "";

}
 
export class Employee

{

    FirstName : string = "";

    LastName : string = "";

    Gender : string = "";

    Email : string = "";

    TermsOfConditions : boolean = false;
 
    // Creating an object of the class ContactDetails{}

    // Create a variable of ContactDetails(-> contactDetails), whose type is ContactDetails, and immediately create an object of ContactDetails.

    contactDetails : ContactDetails = new ContactDetails(); // c# -> ContactDetails contactDetails = new ContactDetails();

}
 
 
employee-form.component.ts

---------------------------

import { Component } from '@angular/core';

import { Employee } from '../models/employee.model';

@Component({

  selector: 'app-employee-form',

  templateUrl: './employee-form.component.html',

  styleUrls: ['./employee-form.component.css']

})

export class EmployeeFormComponent {

  employee : Employee = new Employee(); // new Employee Object(employee) created 

}
 
 
employee-form.component.ts

---------------------------
<form> 
<label>First Name:</label>
<br>
<input type="text" id="firstName" [(ngModel)]="employee.FirstName">
<br>
<label>Last Name:</label>
<br>
<input type="text" id="lastName" [(ngModel)]="employee.LastName">
<br><br>
 
    <!-- GENDER RADIO -->
<label for="gender">Gender:</label>
<br>
<!-- If user selects value="Male" -> employee.Gender = "Male"  -->
<input type="radio" name="Gender" value="Male" [(ngModel)]="employee.Gender">Male
<input type="radio" name="Gender" value="Female" [(ngModel)]="employee.Gender">Female
<br><br>
 
    <!-- EMAIL -->
<label for="email">Email:</label>
<br>
<input type="email" id="email" [(ngModel)]="employee.Email">
<br><br>
 
    <!-- TERMS AND CONDITION CHECKBOX -->
<label>Terms and Conditions:</label>
<br>
<input type="checkbox" id="termsConditions" [(ngModel)]="employee.TermsOfConditions">
<br><br>
 
    <!-- Address -->
<label for="">Address:</label>
<br>
<input type="text" id="address" [(ngModel)]="employee.contactDetails.address">
<br><br>
 
    <!-- Phone -->
<label>Phone:</label>
<br>
<input type="text" id="phone" [(ngModel)]="employee.contactDetails.phone">
<br><br>
 
 
    <button type="submit">Submit</button>
</form>

 
