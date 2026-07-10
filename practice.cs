===========================================================================================================

Models Folder:----------------------------------------------------

============================================================================================================

Course.model.ts---------------


import { Instructor } from './instructor.model';
export interface Course {
  CourseId?: number;
  Title: string;
  Description: string;
  Duration: number;
  InstructorId?: number | null;
  Instructor?: Instructor;
}



Login.model.ts---------------

export interface LoginModel {
    Username?: string;
    Password?: string;
  }


Instructor.model.ts----------------

import { Course } from './course.model';
export interface Instructor {
  InstructorId?: number;
  Name: string;
  Email: string;
  HireDate: Date | string;
  Courses?: Course[];
}


User.model.ts----------------

export interface User {
    Id?: number;
    Username?: string;
    Password?: string;
    Role?: string;
  }





===========================================================================================================

App's Components----------------------------------------------------

============================================================================================================

=======================================
admin.ts-------------------------------


import { Component, OnInit } from '@angular/core';
import { CourseService } from '../services/course.service';
import { InstructorService } from '../services/instructor.service';
import { Course } from '../../models/course.model';
import { Instructor } from '../../models/instructor.model';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit {
  courses: Course[] = [];
  instructors: Instructor[] = [];
  editedCourse: Course | null = null;
  editedInstructor: Instructor | null = null;

  constructor(private courseService: CourseService, private instructorService: InstructorService) {}

  ngOnInit(): void {
    this.getInstructors();
    this.getCourses();
  }

  getInstructors(): void {
    this.instructorService.getInstructors().subscribe(data => this.instructors = data);
  }

  editInstructor(instructor: Instructor): void {
    this.editedInstructor = { ...instructor };
  }

  saveEditedInstructor(instructor: Instructor): void {
    if (instructor.InstructorId) {
      this.instructorService.updateInstructor(instructor.InstructorId, instructor).subscribe(() => {
        this.getInstructors();
        this.editedInstructor = null;
      });
    }
  }

  cancelEditInstructor(): void {
    this.editedInstructor = null;
  }

  deleteInstructor(instructorId: number): void {
    this.instructorService.deleteInstructor(instructorId).subscribe(() => this.getInstructors());
  }

  getCourses(): void {
    this.courseService.getCourses().subscribe(data => this.courses = data);
  }

  editCourse(course: Course): void {
    this.editedCourse = { ...course };
  }

  saveEditedCourse(course: Course): void {
    if (course.CourseId) {
      this.courseService.updateCourse(course.CourseId, course).subscribe(() => {
        this.getCourses();
        this.editedCourse = null;
      });
    }
  }

  cancelEditCourse(): void {
    this.editedCourse = null;
  }

  deleteCourse(courseId: number): void {
    this.courseService.deleteCourse(courseId).subscribe(() => this.getCourses());
  }
}




admin.html---------------------------------------------


<div class="admin-container">
  <h2>Admin Panel</h2>

  <app-instructor
    [instructors]="instructors"
    [editedInstructor]="editedInstructor"
    (editInstructorEvent)="editInstructor($event)"
    (saveEditedInstructorEvent)="saveEditedInstructor($event)"
    (cancelEditInstructorEvent)="cancelEditInstructor()"
    (deleteInstructorEvent)="deleteInstructor($event)">
  </app-instructor>

  <app-course
    [courses]="courses"
    [editedCourse]="editedCourse"
    (editCourseEvent)="editCourse($event)"
    (saveEditedCourseEvent)="saveEditedCourse($event)"
    (cancelEditCourseEvent)="cancelEditCourse()"
    (deleteCourseEvent)="deleteCourse($event)">
  </app-course>

</div>


admin.css:---------------------------------------------

.admin-container { padding: 30px; }



=======================================

make auth guard this=ngs

auth.guard.ts
============================================

import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }

    const requiredRole = route.data['role'];
    if (requiredRole === 'Admin' && !this.authService.isAdmin()) {
      this.router.navigate(['/error']);
      return false;
    }
    if (requiredRole === 'Organizer' && !this.authService.isOrganizer()) {
      this.router.navigate(['/error']);
      return false;
    }

    return true;
  }
}


====================================================================
course.comp.ts:


import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Course } from '../../models/course.model';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html'
})
export class CourseComponent {
  @Input() courses: Course[] = [];
  @Input() editedCourse: Course | null = null;

  @Output() editCourseEvent = new EventEmitter<Course>();
  @Output() saveEditedCourseEvent = new EventEmitter<Course>();
  @Output() cancelEditCourseEvent = new EventEmitter<void>();
  @Output() deleteCourseEvent = new EventEmitter<number>();

  onEditCourse(course: Course): void {
    this.editCourseEvent.emit(course);
  }

  onSaveEditedCourse(): void {
    if (this.editedCourse) {
      this.saveEditedCourseEvent.emit(this.editedCourse);
    }
  }

  onCancelEditCourse(): void {
    this.cancelEditCourseEvent.emit();
  }

  onDeleteCourse(courseId: number): void {
    this.deleteCourseEvent.emit(courseId);
  }
}




course.comp.html;-----------------------------------


<div class="course-list">
  <h3>Course List</h3>
  <table>
    <tr>
      <th>Title</th>
      <th>Description</th>
      <th>Duration</th>
      <th>Actions</th>
    </tr>
    <tr *ngFor="let course of courses">
      <td>{{course.Title}}</td>
      <td>{{course.Description}}</td>
      <td>{{course.Duration}} days</td>
      <td>
        <button class="edit-btn" (click)="onEditCourse(course)">Edit</button>
        <button class="delete-btn" (click)="onDeleteCourse(course.CourseId!)">Delete</button>
      </td>
    </tr>
  </table>

<div *ngIf="editedCourse" class="edit-form">
    <h3>Edit Course</h3>
    <label>Title:</label>
    <input type="text" [(ngModel)]="editedCourse.Title">

    <label>Description:</label>
    <textarea [(ngModel)]="editedCourse.Description"></textarea>

    <label>Duration (in days):</label>
    <input type="number" [(ngModel)]="editedCourse.Duration">

    <button (click)="onSaveEditedCourse()">Save</button>
    <button (click)="onCancelEditCourse()">Cancel</button>

</div>
</div>



course.csss----------------------------------------------------

.course-container { padding: 20px; }


==================================================================

====================================================================

create-course.comp.ts:------------------------------------------


import { Component } from '@angular/core';
import { CourseService } from '../../services/course.service';
import { Router } from '@angular/router';
import { Course } from '../../../models/course.model';

@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html'
})
export class CreateCourseComponent {
  newCourse: Course = { Title: '', Description: '', Duration: 0 };

 
  tDirty = false;
  dDirty = false;
  durDirty = false;

  get durationStatus(): string {
    if (!this.newCourse.Duration || this.newCourse.Duration === 0) return 'Undefined';
    if (this.newCourse.Duration < 10) return 'Short Duration';
    if (this.newCourse.Duration >= 10 && this.newCourse.Duration <= 30) return 'Moderate Duration';
    return 'Long Duration';
  }

  constructor(private courseService: CourseService, private router: Router) {}

  createCourse(): void {
    this.courseService.createCourse(this.newCourse).subscribe(() => {
      this.newCourse = { Title: '', Description: '', Duration: 0 };
      this.router.navigate(['/admin']);
    });
  }
}




create-course.comp.html:------------------------------------------

<div class="create-course-container">
  <h2>CREATE NEW COURSE</h2>
  <form #courseForm="ngForm" (ngSubmit)="courseForm.valid && createCourse()">
    <label>Course Title *</label>
    <input type="text" id="courseTitle" name="courseTitle" [(ngModel)]="newCourse.Title" required minlength="2"
      maxlength="100" #titleInp (input)="tDirty=true">
    <div class="error-message" *ngIf="tDirty && titleInp.value === ''">Course Title is required</div>

    <label>Description *</label>
    <textarea id="courseDescription" name="courseDescription" [(ngModel)]="newCourse.Description" required #descInp
      (input)="dDirty=true"></textarea>
    <div class="error-message" *ngIf="dDirty && descInp.value === ''">Description is required</div>

    <label>Duration (in days) *</label>
    <input type="number" id="courseDuration" name="courseDuration" [(ngModel)]="newCourse.Duration" required #durInp
      (input)="durDirty=true">
    <div class="error-message" *ngIf="durDirty && durInp.value === ''">Duration is required</div>

    <div id="status">Duration Status: {{ durationStatus }}</div>

    <button type="submit" id="submit" [disabled]="courseForm.invalid">CREATE</button>

  </form>
</div>



creatcourse-css------------------------------------------------

.form-container {
    max-width: 600px; margin: 40px auto; padding: 30px;
    background: #fff; border-radius: 12px;
    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  }
  .title { color: #1e90ff; text-align: center; font-weight: bold; }
  label { display: block; margin-top: 15px; color: #333; }
  .required { color: red; }
  input, textarea {
    width: 100%; padding: 10px; margin-top: 6px;
    border: 1px solid #ccc; border-radius: 6px; box-sizing: border-box;
  }
  textarea { min-height: 100px; resize: vertical; }
  .error-message { color: #e74c3c; font-style: italic; margin-top: 4px; font-size: 14px; }
  .duration-status { color: #28a745; text-align: center; margin: 15px 0; }
  button {
    width: 100%; padding: 12px;
    background-color: #1e90ff; color: white; border: none;
    border-radius: 6px; font-size: 16px; font-weight: bold; cursor: pointer;
  }
  button:disabled { background-color: #a0c8f0; cursor: not-allowed; }
  



===========================================================================================
============================================================================================

error.comp.ts:---------------------


import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html'
})
export class ErrorComponent { }



error.comp.html:---------------------


<div class="error-container">
  <h2>Oops! Something went wrong.</h2>
</div>


erro.comp.css-----------------------------

.error-container { text-align: center; padding: 50px; color: #e74c3c; }




===========================================================================================
============================================================================================


home com ts----------------------

import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent { }



home comp html--------------------

<div class="home-container">
  <h2>WELCOME TO THE COURSE-INSTRUCTOR MANAGEMENT SYSTEM</h2>
  <p>This is the home page of our Course-Instructor Management System. Use the navigation menu to manage instructors, courses, and their assignments efficiently.</p>
</div>



hom csss-----------------------------

.home-container { text-align: center; padding: 50px; }




===========================================================================================
============================================================================================


instruct.comp.ts_______________________



import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Instructor } from '../../models/instructor.model';

@Component({
  selector: 'app-instructor',
  templateUrl: './instructor.component.html'
})
export class InstructorComponent {
  @Input() instructors: Instructor[] = [];
  @Input() editedInstructor: Instructor | null = null;

  @Output() editInstructorEvent = new EventEmitter<Instructor>();
  @Output() saveEditedInstructorEvent = new EventEmitter<Instructor>();
  @Output() cancelEditInstructorEvent = new EventEmitter<void>();
  @Output() deleteInstructorEvent = new EventEmitter<number>();

  onEditInstructor(instructor: Instructor): void {
    this.editInstructorEvent.emit(instructor);
  }

  onSaveEditedInstructor(): void {
    if (this.editedInstructor) {
      this.saveEditedInstructorEvent.emit(this.editedInstructor);
    }
  }

  onCancelEditInstructor(): void {
    this.cancelEditInstructorEvent.emit();
  }

  onDeleteInstructor(instructorId: number): void {
    this.deleteInstructorEvent.emit(instructorId);
  }

  onHireDateChange(date: string): void {
    if (this.editedInstructor) {
      this.editedInstructor.HireDate = date;
    }
  }
}



instruct.comp.html-_______________________


<div class="instructor-list">
  <h3>Instructor List</h3>
  <table>
    <tr>
      <th>Instructor Name</th>
      <th>Email</th>
      <th>Hire Date</th>
      <th>Actions</th>
    </tr>
    <tr *ngFor="let inst of instructors">
      <td>{{inst.Name}}</td>
      <td>{{inst.Email}}</td>
      <td>{{inst.HireDate | date}}</td>
      <td>
        <button class="edit-btn" (click)="onEditInstructor(inst)">Edit</button>
        <button class="delete-btn" (click)="onDeleteInstructor(inst.InstructorId!)">Delete</button>
      </td>
    </tr>
  </table>

<div *ngIf="editedInstructor" class="edit-form">
    <h3>Edit Instructor</h3>
    <label>Instructor Name:</label>
    <input type="text" [(ngModel)]="editedInstructor.Name">

    <label>Email:</label>
    <input type="text" [(ngModel)]="editedInstructor.Email">

    <label>Hire Date:</label>
    <input type="date" [ngModel]="editedInstructor.HireDate | date:'yyyy-MM-dd'" (ngModelChange)="onHireDateChange($event)">

    <button (click)="onSaveEditedInstructor()">Save</button>
    <button (click)="onCancelEditInstructor()">Cancel</button>

</div>
</div>



instructor.css--------------------------------------


.instructor-container { padding: 20px; }



===========================================================================================
============================================================================================


create-instructor.cmp.ts----------------


import { Component } from '@angular/core';
import { InstructorService } from '../../services/instructor.service';
import { Router } from '@angular/router';
import { Instructor } from '../../../models/instructor.model';

@Component({
  selector: 'app-create-instructor',
  templateUrl: './create-instructor.component.html'
})
export class CreateInstructorComponent {
  newInstructor: Instructor = { Name: '', Email: '', HireDate: '' };

  constructor(private instructorService: InstructorService, private router: Router) {}

  createInstructor(): void {
    this.instructorService.createInstructor(this.newInstructor).subscribe(() => {
      this.newInstructor = { Name: '', Email: '', HireDate: '' };
      this.router.navigate(['/admin']);
    });
  }
}


create-instructor.cmp.html ----------------


<div class="create-instructor-container">
  <h2>CREATE NEW INSTRUCTOR</h2>
  <form #instructorForm="ngForm" (ngSubmit)="instructorForm.valid && createInstructor()">

    <label>Instructor Name *</label>
    <input type="text" id="instructorName" name="instructorName" [(ngModel)]="newInstructor.Name" required minlength="2" maxlength="100" #iName="ngModel">
    <div class="error-message" *ngIf="iName.invalid && (iName.dirty || iName.touched)">
      Instructor Name is required
    </div>

    <label>Email *</label>
    <input type="email" id="email" name="email" [(ngModel)]="newInstructor.Email" required email #iEmail="ngModel">
    <div class="error-message" *ngIf="iEmail.invalid && (iEmail.dirty || iEmail.touched)">
      Email is required
    </div>

    <label>Hire Date *</label>
    <input type="date" id="hireDate" name="hireDate" [(ngModel)]="newInstructor.HireDate" required #iDate="ngModel">
    <div class="error-message" *ngIf="iDate.invalid && (iDate.dirty || iDate.touched)">
      Hire Date is required
    </div>

    <button type="submit" id="submit" [disabled]="instructorForm.invalid">CREATE</button>

</form>
</div>



create-instructor.css-------------------------------


.form-container {
    max-width: 600px; margin: 40px auto; padding: 30px;
    background: #fff; border-radius: 12px;
    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  }
  .title { color: #1e90ff; text-align: center; font-weight: bold; }
  label { display: block; margin-top: 15px; color: #333; }
  .required { color: red; }
  input {
    width: 100%; padding: 10px; margin-top: 6px;
    border: 1px solid #ccc; border-radius: 6px; box-sizing: border-box;
  }
  .error-message { color: #e74c3c; font-style: italic; margin-top: 4px; font-size: 14px; }
  button {
    width: 100%; padding: 12px; margin-top: 25px;
    background-color: #1e90ff; color: white; border: none;
    border-radius: 6px; font-size: 16px; font-weight: bold; cursor: pointer;
  }
  button:disabled { background-color: #a0c8f0; cursor: not-allowed; }
  


===========================================================================================
============================================================================================



login.comp.ts_______________________

import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { LoginModel } from '../../models/login-model.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  loginModel: LoginModel = { Username: '', Password: '' };

  
  uDirty = false;
  pDirty = false;

  private routeUserBasedOnRole(): void {
    if (this.authService.isAdmin()) {
      this.router.navigate(['/admin']);
    } else if (this.authService.isOrganizer()) {
      this.router.navigate(['/organizer']);
    }
  }

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    this.authService.login(this.loginModel).subscribe({
      next: () => this.routeUserBasedOnRole(),
      error: (err) => console.error(err)
    });
  }
}



========================================================================

login.comp.html_______________


<div class="login-container">
  <h2>Login</h2>
  <form #loginForm="ngForm" (ngSubmit)="loginForm.valid && login()">

    <label>Username*</label>
    <input type="text" id="username" name="username" [(ngModel)]="loginModel.Username" required #userInp (input)="uDirty=true">
    <div class="error-message" *ngIf="uDirty && userInp.value === ''">Username is required</div>

    <label>Password*</label>
    <input type="password" id="password" name="password" [(ngModel)]="loginModel.Password" required #passInp (input)="pDirty=true">
    <div class="error-message" *ngIf="pDirty && passInp.value === ''">Password is required</div>

    <button type="submit" id="submit" [disabled]="loginForm.invalid">Login</button>

</form>
</div>




login.css__________________________


.form-container {
    max-width: 500px; margin: 40px auto; padding: 30px;
    background: #fff; border-radius: 12px;
    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  }
  .title { color: #1e90ff; font-weight: bold; }
  label { display: block; margin-top: 15px; color: #333; }
  .required { color: red; }
  input {
    width: 100%; padding: 10px; margin-top: 6px;
    border: 1px solid #e74c3c; border-radius: 4px; box-sizing: border-box;
  }
  .error-message { color: #e74c3c; font-style: italic; margin-top: 4px; font-size: 14px; }
  button {
    padding: 10px 24px; margin-top: 20px;
    background-color: #1e90ff; color: white; border: none;
    border-radius: 4px; font-size: 16px; cursor: pointer;
  }
  button:disabled { background-color: #a0c8f0; cursor: not-allowed; }
  



===========================================================================================
============================================================================================



navbar.comp.ts____________________________________________

import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {

  constructor(public authService: AuthService, private router: Router) {}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}


navbar.comp.html_______________________________________

<nav class="navbar">
  <div class="brand">Course-Instructor Management System</div>
  <ul class="nav-links">
    <li><a routerLink="/">Home</a></li>

    <li *ngIf="authService.isOrganizer()"><a routerLink="/organizer">Organizer</a></li>

    <li *ngIf="authService.isAdmin()"><a routerLink="/admin">Admin</a></li>
    <li *ngIf="authService.isAdmin()"><a routerLink="/admin/createInstructor">Create Instructor</a></li>
    <li *ngIf="authService.isAdmin()"><a routerLink="/admin/createCourse">Create Course</a></li>

    <li *ngIf="!authService.isLoggedIn()"><a routerLink="/signup">Register</a></li>
    <li *ngIf="!authService.isLoggedIn()"><a routerLink="/login">Login</a></li>

    <li *ngIf="authService.isLoggedIn()"><a style="cursor: pointer;" (click)="logout()">Logout</a></li>

</ul>
</nav>



navbar.css----------------------------------


.navbar {
    display: flex; justify-content: space-between; align-items: center;
    background-color: #1e90ff; padding: 15px 30px; color: white;
  }
  .brand {
    font-size: 22px; font-weight: bold;
    font-family: 'Times New Roman', serif;
  }
  .nav-links {
    list-style: none; display: flex; gap: 30px; margin: 0; padding: 0;
  }
  .nav-links a {
    color: white; text-decoration: none; font-size: 16px;
  }
  .nav-links a:hover { text-decoration: underline; }
  


===========================================================================================
============================================================================================


organizer.comp.ts_____________________________________________


import { Component, OnInit } from '@angular/core';
import { CourseService } from '../services/course.service';
import { InstructorService } from '../services/instructor.service';
import { Course } from '../../models/course.model';
import { Instructor } from '../../models/instructor.model';

@Component({
  selector: 'app-organizer',
  templateUrl: './organizer.component.html'
})
export class OrganizerComponent implements OnInit {
  courses: Course[] = [];
  unassignedCourses: Course[] = [];
  instructors: Instructor[] = [];

  constructor(private courseService: CourseService, private instructorService: InstructorService) {}

  ngOnInit(): void {
    this.getCourses();
    this.getInstructors();
  }

  getCourses(): void {
    this.courseService.getCourses().subscribe(data => {
      this.courses = data;
      this.unassignedCourses = data.filter(c => !c.InstructorId);
    });
  }

  getInstructors(): void {
    this.instructorService.getInstructors().subscribe(data => this.instructors = data);
  }

  assignCourseToInstructor(course: Course, selectedInstructorId: number): void {
    course.InstructorId = selectedInstructorId;
    if (course.CourseId) {
      this.courseService.updateCourse(course.CourseId, course).subscribe(() => {
        this.getCourses();
        this.getInstructors();
      });
    }
  }

  releaseCourseFromInstructor(course: Course): void {
    course.InstructorId = null;
    if (course.CourseId) {
      this.courseService.updateCourse(course.CourseId, course).subscribe(() => {
        this.getCourses();
        this.getInstructors();
      });
    }
  }
}



organizer.comp.html---------------------------------------------

<div class="organizer-container">
  <h2>INSTRUCTOR-COURSE ASSIGNMENT PANEL</h2>

<h3>Unassigned Courses</h3>
  <table *ngIf="unassignedCourses.length > 0; else noUnassigned">
    <thead>
      <tr>
        <th>Title</th>
        <th>Description</th>
        <th>Duration</th>
        <th>Instructor</th>
        <th>Action</th>
      </tr>
    </thead>
    <tbody>
      <tr *ngFor="let course of unassignedCourses">
        <td>{{course.Title}}</td>
        <td>{{course.Description}}</td>
        <td>{{course.Duration}} days</td>
        <td>
          <select [(ngModel)]="course.InstructorId">
            <option *ngFor="let inst of instructors" [value]="inst.InstructorId">{{inst.Name}}</option>
          </select>
        </td>
        <td>
          <button (click)="assignCourseToInstructor(course, course.InstructorId!)">Assign to Instructor</button>
        </td>
      </tr>
    </tbody>
  </table>
  <ng-template #noUnassigned>
    <p id="no_unassigned">No Unassigned Courses</p>
  </ng-template>

<h3>Instructor List With Courses</h3>
  <table *ngIf="instructors.length > 0; else noInstructors">
    <thead>
      <tr>
        <th>Instructor Name</th>
        <th>Email</th>
        <th>Courses</th>
      </tr>
    </thead>
    <tbody>
      <tr *ngFor="let inst of instructors">
        <td>{{inst.Name}}</td>
        <td>{{inst.Email}}</td>
        <td>
          <table>
            <tbody>
              <tr *ngFor="let course of inst.Courses">
                <td>{{course.Title}}</td>
                <td>{{course.Duration}} days</td>
                <td><button (click)="releaseCourseFromInstructor(course)">Release Course</button></td>
              </tr>
            </tbody>
          </table>
        </td>
      </tr>
    </tbody>
  </table>
  <ng-template #noInstructors>
    <p id="no_instructors">No Instructors Available</p>
  </ng-template>
</div>




organizer.css_____________________________________________

.organizer-container { padding: 30px; }


======================================================================================================

======================================================================================================


register.comp.ts------------------------------------------------------


import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html'
})
export class RegistrationComponent {
  user: User = { Username: '', Password: '', Role: '' };
  confirmPassword = '';
  A: string[] = ['Admin', 'Organizer'];

  
  uDirty = false;
  pDirty = false;
  cDirty = false;
  rDirty = false;

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    if (this.user.Password !== this.confirmPassword) return;

    this.authService.register(this.user).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (err) => console.error(err)
    });
  }
}



register.comp.html-----------------------------------------------------------


<div class="registration-container">
  <h2>REGISTRATION</h2>
  <form #regForm="ngForm" (ngSubmit)="regForm.valid && user.Password === confirmPassword && register()">

    <label>Username *</label>
    <input type="text" id="username" name="username" [(ngModel)]="user.Username" required #userInp
      (input)="uDirty=true">
    <div class="error-message" *ngIf="uDirty && userInp.value === ''">Username is required</div>

    <label>Password *</label>
    <input type="password" id="password" name="password" [(ngModel)]="user.Password" required
      pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$" #passInp (input)="pDirty=true">
    <div class="error-message" *ngIf="pDirty && passInp.value === ''">Password is required</div>

    <label>Confirm Password *</label>
    <input type="password" id="confirmPassword" name="confirmPassword" [(ngModel)]="confirmPassword" required
      #confirmInp (input)="cDirty=true">
    <div class="error-message" *ngIf="cDirty && confirmInp.value === ''">Confirm Password is required</div>
    <div class="error-message" *ngIf="cDirty && confirmInp.value !== '' && confirmInp.value !== passInp.value">Passwords
      do not match</div>

    <label>Role *</label>
    <select id="role" name="role" [(ngModel)]="user.Role" required #roleInp (change)="rDirty=true">
      <option value="">Select a role</option>
      <option *ngFor="let r of A" [value]="r">{{r}}</option>
    </select>
    <div class="error-message" *ngIf="rDirty && roleInp.value === ''">Role is required</div>

    <button type="submit" id="submit"
      [disabled]="regForm.invalid || confirmPassword !== user.Password">REGISTER</button>

  </form>
</div>



register.csss------------------------------------------------------

.form-container {
    max-width: 600px;
    margin: 40px auto;
    padding: 30px;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  }
  .title { color: #1e90ff; text-align: center; font-weight: bold; }
  label { display: block; margin-top: 15px; color: #333; }
  .required { color: red; }
  input, select {
    width: 100%; padding: 10px; margin-top: 6px;
    border: 1px solid #ccc; border-radius: 6px; box-sizing: border-box;
  }
  .error-message { color: #e74c3c; font-style: italic; margin-top: 4px; font-size: 14px; }
  button {
    width: 100%; padding: 12px; margin-top: 25px;
    background-color: #1e90ff; color: white; border: none;
    border-radius: 6px; font-size: 16px; font-weight: bold; cursor: pointer;
  }
  button:disabled { background-color: #c0c0c0; cursor: not-allowed; }
  



=========================================================================================
==========================================================================================


services folder----------------------------------------------------------
================================================================================


auth.service.ts----------------------------
-----------------------------------------------

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { User } from '../../models/user.model';
import { LoginModel } from '../../models/login-model.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  public baseUrl = 'https://8080-adffaebecead351852509adcdcbdcfaone.premiumproject.examly.io/api/users';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isLoggedIn());

  private storeUserData(userData: any): void {
    if (!userData) return;
    const role = userData.role || userData.Role;
    if (role) {
      localStorage.setItem('role', role);
    }
    localStorage.setItem('token', 'auth-token');
  }

  constructor(private http: HttpClient) {}

  register(newUser: User): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/register`, newUser).pipe(
      tap(res => this.storeUserData(res))
    );
  }

  login(loginUser: LoginModel): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, loginUser).pipe(
      tap(res => {
        // Handles both the real .NET wrapper {user: ...} and the flat mock {role: ...}
        const userData = res && res.user ? res.user : res;
        this.storeUserData(userData);
        this.updateAuthenticationStatus(true);
      })
    );
  }

  logout(): void {
    localStorage.clear();
    this.updateAuthenticationStatus(false);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('role');
  }

  isAdmin(): boolean {
    return localStorage.getItem('role') === 'Admin';
  }

  isOrganizer(): boolean {
    return localStorage.getItem('role') === 'Organizer';
  }

  updateAuthenticationStatus(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }
}


course.service.ts----------------------------
----------------------------------------------



import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from '../../models/course.model';

@Injectable({ providedIn: 'root' })
export class CourseService {
  public baseUrl = 'https://8080-adffaebecead351852509adcdcbdcfaone.premiumproject.examly.io/api/Course';

  constructor(private http: HttpClient) {}

  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.baseUrl}/GetCourses`);
  }

  createCourse(course: Course): Observable<Course> {
    return this.http.post<Course>(`${this.baseUrl}/PostCourse`, course);
  }

  updateCourse(courseId: number, course: Course): Observable<Course> {
    return this.http.put<Course>(`${this.baseUrl}/PutCourse/${courseId}`, course);
  }

  deleteCourse(courseId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeleteCourse/${courseId}`);
  }
}




instructor.sercice.ts-------------------------
----------------------------------------------


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Instructor } from '../../models/instructor.model';

@Injectable({ providedIn: 'root' })
export class InstructorService {
  public baseUrl = 'https://8080-adffaebecead351852509adcdcbdcfaone.premiumproject.examly.io/api/Instructor';

  constructor(private http: HttpClient) {}

  getInstructors(): Observable<Instructor[]> {
    return this.http.get<Instructor[]>(`${this.baseUrl}/GetInstructors`);
  }

  createInstructor(instructor: Instructor): Observable<Instructor> {
    return this.http.post<Instructor>(`${this.baseUrl}/PostInstructor`, instructor);
  }

  updateInstructor(instructorId: number, instructor: Instructor): Observable<Instructor> {
    return this.http.put<Instructor>(`${this.baseUrl}/PutInstructor/${instructorId}`, instructor);
  }

  deleteInstructor(instructorId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeleteInstructor/${instructorId}`);
  }
}


========================================================
app.module.ts-----------------------------------------


import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { OrganizerComponent } from './organizer/organizer.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { ErrorComponent } from './error/error.component';
import { CourseComponent } from './course/course.component';
import { InstructorComponent } from './instructor/instructor.component';
import { CreateCourseComponent } from './course/create-course/create-course.component';
import { CreateInstructorComponent } from './instructor/create-instructor/create-instructor.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    AdminComponent,
    OrganizerComponent,
    LoginComponent,
    RegistrationComponent,
    ErrorComponent,
    CourseComponent,
    InstructorComponent,
    CreateCourseComponent,
    CreateInstructorComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }



==========================================================
app.cmp.ts don't touch
=========================================================
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
}


=============================================================
app.comp.html
============================================================
<app-navbar></app-navbar>
<router-outlet></router-outlet>


================================================================
app.routing.model.ts
================================================================

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { CreateCourseComponent } from './course/create-course/create-course.component';
import { CreateInstructorComponent } from './instructor/create-instructor/create-instructor.component';
import { OrganizerComponent } from './organizer/organizer.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { ErrorComponent } from './error/error.component';
import { AuthGuard } from './authguard/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard], data: { role: 'Admin' } },
  { path: 'admin/createCourse', component: CreateCourseComponent, canActivate: [AuthGuard], data: { role: 'Admin' } },
  { path: 'admin/createInstructor', component: CreateInstructorComponent, canActivate: [AuthGuard], data: { role: 'Admin' } },
  { path: 'organizer', component: OrganizerComponent, canActivate: [AuthGuard], data: { role: 'Organizer' } },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: RegistrationComponent },
  { path: 'error', component: ErrorComponent },
  { path: '**', redirectTo: '/error' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }



