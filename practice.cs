
Previewing Increment 3 2 3.md
Admin CSS
.admin-container {

width: 90%;

margin: 30px auto;

padding: 30px;

}


h1 {

text-align: center;

font-size: 42px;

margin-bottom: 40px;

}


Admin Html

Admin Panel
<app-instructor [instructors]="instructors" [editedInstructor]="editedInstructor"

(editInstructorEvent)="editInstructor($event)" (saveEditedInstructorEvent)="saveEditedInstructor()"

(cancelEditInstructorEvent)="cancelEditInstructor()" (deleteInstructorEvent)="deleteInstructor($event)">


<app-course [courses]="courses" [editedCourse]="editedCourse" (editCourseEvent)="editCourse($event)"

(saveEditedCourseEvent)="saveEditedCourse()" (cancelEditCourseEvent)="cancelEditCourse()"

(deleteCourseEvent)="deleteCourse($event)">


Admin Component
import { Component, OnInit } from '@angular/core';

import { Instructor } from '../../models/instructor.model';

import { Course } from '../../models/course.model';

import { InstructorService } from '../services/instructor.service';

import { CourseService } from '../services/course.service';

@Component({

selector: 'app-admin',

templateUrl: './admin.component.html',

styleUrls: ['./admin.component.css']

})

export class AdminComponent implements OnInit {

instructors: Instructor[] = [];

courses: Course[] = [];

editedInstructor: Instructor | null = null;

editedCourse: Course | null = null;

constructor(

private instructorService: InstructorService,

private courseService: CourseService

) { }

ngOnInit(): void {

this.getInstructors();

this.getCourses();

}

getInstructors(): void {

this.instructorService.getInstructors().subscribe({

next: (data) => {

this.instructors = data;

},

error: (error) => {

console.log(error);

}

});

}

editInstructor(instructor: Instructor): void {

this.editedInstructor = { ...instructor };

}

saveEditedInstructor(): void {

if (this.editedInstructor && this.editedInstructor.InstructorId) {

this.instructorService

.updateInstructor(this.editedInstructor.InstructorId, this.editedInstructor)

.subscribe({

next: () => {

this.getInstructors();

this.editedInstructor = null;

},

error: (error) => {

console.log(error);

}

});

}

}

cancelEditInstructor(): void {

this.editedInstructor = null;

}

deleteInstructor(instructorId: number): void {

this.instructorService.deleteInstructor(instructorId).subscribe({

next: () => {

this.getInstructors();

},

error: (error) => {

console.log(error);

}

});

}

getCourses(): void {

this.courseService.getCourses().subscribe({

next: (data) => {

this.courses = data;

},

error: (error) => {

console.log(error);

}

});

}

editCourse(course: Course): void {

this.editedCourse = { ...course };

}

saveEditedCourse(): void {

if (this.editedCourse && this.editedCourse.CourseId) {

this.courseService

.updateCourse(this.editedCourse.CourseId, this.editedCourse)

.subscribe({

next: () => {

this.getCourses();

this.editedCourse = null;

},

error: (error) => {

console.log(error);

}

});

}

}

cancelEditCourse(): void {

this.editedCourse = null;

}

deleteCourse(courseId: number): void {

this.courseService.deleteCourse(courseId).subscribe({

next: () => {

this.getCourses();

},

error: (error) => {

console.log(error);

}

});

}

}

==========================================================================

Authguard.ts
import { Injectable } from '@angular/core';

import {

CanActivate,

ActivatedRouteSnapshot,

Router,

RouterStateSnapshot

} from '@angular/router';

import { AuthService } from '../services/auth.service';

@Injectable({

providedIn: 'root'

})

export class AuthGuard implements CanActivate {

constructor(

private authService: AuthService,

private router: Router

) { }

canActivate(

route: ActivatedRouteSnapshot,

state: RouterStateSnapshot

): boolean {

if (!this.authService.isLoggedIn()) {

this.router.navigate(['/login']);

return false;

}

if (state.url.includes('/admin') && !this.authService.isAdmin()) {

this.router.navigate(['/error']);

return false;

}

if (state.url.includes('/organizer') && !this.authService.isOrganizer()) {

this.router.navigate(['/error']);

return false;

}

return true;

}

}

===========================================================================

Course CSS
.course-section {

margin-top: 50px;

margin-bottom: 50px;

}

h2 {

font-size: 34px;

margin-bottom: 20px;

color: #333333;

}

table {

width: 100%;

border-collapse: collapse;

box-shadow: 0 0 12px lightgray;

border-radius: 8px;

overflow: hidden;

margin-bottom: 40px;

}

thead {

background-color: #0d85f2;

color: white;

}

th,

td {

padding: 18px;

text-align: left;

font-size: 18px;

}

th {

font-weight: bold;

}

.edit-btn {

background-color: #28a745;

color: white;

border: none;

padding: 8px 18px;

margin-right: 8px;

border-radius: 5px;

font-size: 16px;

cursor: pointer;

}

.delete-btn {

background-color: #dc3545;

color: white;

border: none;

padding: 8px 18px;

border-radius: 5px;

font-size: 16px;

cursor: pointer;

}

.edit-card {

width: 650px;

margin: 40px auto;

padding: 35px;

box-shadow: 0 0 15px lightgray;

border-radius: 10px;

}

.edit-card h2 {

text-align: center;

color: #333333;

margin-bottom: 25px;

}

label {

display: block;

font-weight: bold;

margin-top: 15px;

margin-bottom: 8px;

font-size: 18px;

}

input,

textarea {

width: 100%;

padding: 12px;

font-size: 16px;

box-sizing: border-box;

}

textarea {

height: 90px;

}

.duration-status {

width: 400px;

margin: 30px auto;

padding: 20px;

box-shadow: 0 0 12px lightgray;

border-radius: 8px;

font-size: 18px;

}

.button-group {

margin-top: 25px;

}

.save-btn {

background-color: #0d85f2;

color: white;

border: none;

padding: 10px 22px;

margin-right: 8px;

border-radius: 5px;

font-size: 16px;

cursor: pointer;

}

.cancel-btn {

background-color: #6c757d;

color: white;

border: none;

padding: 10px 22px;

border-radius: 5px;

font-size: 16px;

cursor: pointer;

}

Course Html

Course List










<tr *ngFor="let course of courses">








TITLE	DESCRIPTION	DURATION	ACTIONS
{{ course.Title }}	{{ course.Description }}	{{ course.Duration }} days	
<button class="edit-btn" (click)="onEditCourse(course)">

Edit


<button class="delete-btn" (click)="onDeleteCourse(course.CourseId || 0)">

Delete


<div class="edit-card" *ngIf="editedCourse">


Edit Course
Title:

<input type="text" [(ngModel)]="editedCourse.Title">

Description:

<textarea [(ngModel)]="editedCourse.Description">


Duration:

<input type="number" [(ngModel)]="editedCourse.Duration">


Duration Status: {{ getDurationStatus(editedCourse.Duration) }}



<button class="save-btn" (click)="onSaveEditedCourse()">

Save


<button class="cancel-btn" (click)="onCancelEditCourse()">

Cancel



Course Component
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { Course } from '../../models/course.model';

@Component({

selector: 'app-course',

templateUrl: './course.component.html',

styleUrls: ['./course.component.css']

})

export class CourseComponent {

@Input() courses: Course[] = [];

@Input() editedCourse: Course | null = null;

@Output() editCourseEvent = new EventEmitter();

@Output() saveEditedCourseEvent = new EventEmitter();

@Output() cancelEditCourseEvent = new EventEmitter();

@Output() deleteCourseEvent = new EventEmitter();

onEditCourse(course: Course): void {

this.editCourseEvent.emit(course);

}

onSaveEditedCourse(): void {

this.saveEditedCourseEvent.emit();

}

onCancelEditCourse(): void {

this.cancelEditCourseEvent.emit();

}

onDeleteCourse(courseId: number): void {

this.deleteCourseEvent.emit(courseId);

}

getDurationStatus(duration: number): string {

if (duration < 10) {

return 'Short Duration';

}

if (duration >= 10 && duration <= 30) {

return 'Moderate Duration';

}

return 'Long Duration';

}

}

Create-Course CSS
form {

width: 700px;

margin: 30px auto;

}

h1 {

text-align: center;

color: #0d6efd;

}

label {

display: block;

margin-top: 15px;

margin-bottom: 5px;

font-weight: bold;

}

input,

textarea {

width: 100%;

padding: 10px;

}

textarea {

height: 100px;

}

.error-message {

color: red;

margin-top: 5px;

margin-bottom: 10px;

}

#status {

margin-top: 15px;

color: green;

font-weight: bold;

text-align: center;

}

button {

width: 100%;

padding: 10px;

margin-top: 15px;

background-color: #0d6efd;

color: white;

border: none;

}

Create-Course Html

CREATE NEW COURSE
Course Title

<input type="text" id="courseTitle" name="courseTitle" [(ngModel)]="newCourse.Title" required

#courseTitleRef="ngModel" (input)="lastField = 'courseTitle'">

Description

<textarea id="courseDescription" name="courseDescription" [(ngModel)]="newCourse.Description" required

#courseDescriptionRef="ngModel" (input)="lastField = 'courseDescription'">


Duration

<input type="number" id="courseDuration" name="courseDuration" [(ngModel)]="newCourse.Duration" required

#courseDurationRef="ngModel" (input)="lastField = 'courseDuration'">


{{ getCourseErrorMessage() }}



Duration Status: {{ durationStatus }}


<button type="submit" id="submit" [disabled]="courseForm.invalid">

CREATE


Create-Course Component
import { Component } from '@angular/core';

import { Router } from '@angular/router';

import { CourseService } from '../../services/course.service';

import { Course } from '../../../models/course.model';

@Component({

selector: 'app-create-course',

templateUrl: './create-course.component.html',

styleUrls: ['./create-course.component.css']

})

export class CreateCourseComponent {

newCourse: Course = {

Title: '',

Description: '',

Duration: 0,

InstructorId: null

};

errorMessage: string = '';

lastField: string = '';

constructor(

private courseService: CourseService,

private router: Router

) { }

createCourse(): void {

this.errorMessage = '';

this.courseService.createCourse(this.newCourse).subscribe({

next: () => {

this.router.navigate(['/admin']);

},

error: () => {

this.errorMessage = 'Course not added';

}

});

}

get durationStatus(): string {

if (!this.newCourse.Duration) {

return 'Undefined';

}

if (this.newCourse.Duration < 10) {

return 'Short Duration';

}

if (this.newCourse.Duration >= 10 && this.newCourse.Duration <= 30) {

return 'Moderate Duration';

}

return 'Long Duration';

}

getCourseErrorMessage(): string {

if (this.lastField === 'courseDescription' && this.newCourse.Description === '') {

return 'Description is required';

}

if (this.lastField === 'courseTitle' && this.newCourse.Title === '') {

return 'Course Title is required';

}

if (this.lastField === 'courseDuration' && !this.newCourse.Duration) {

return 'Duration is required';

}

return '';

}

}

Error CSS
.error-container {

display: flex;

justify-content: center;

align-items: center;

height: 80vh;

}

.error-card {

text-align: center;

padding: 40px;

box-shadow: 0 0 15px lightgray;

border-radius: 10px;

width: 500px;

}

.error-card h1 {

font-size: 70px;

color: #dc3545;

margin-bottom: 10px;

}

.error-card h2 {

margin-bottom: 15px;

}

.error-card p {

margin-bottom: 20px;

}

.error-card a {

text-decoration: none;

background-color: #0d85f2;

color: white;

padding: 10px 20px;

border-radius: 5px;

}

Error html


404

Page Not Found

The page you are looking for does not exist.



Go to Home



Home CSS
.home-container {

min-height: 500px;

display: flex;

justify-content: center;

align-items: flex-start;

padding-top: 90px;

font-family: Georgia, serif;

}

.welcome-card {

width: 720px;

padding: 45px 55px;

text-align: center;

border-radius: 8px;

box-shadow: 0 0 16px lightgray;

background-color: white;

}

.welcome-card h1 {

color: #0d85f2;

font-size: 34px;

line-height: 1.3;

margin-bottom: 30px;

font-weight: bold;

}

.welcome-card p {

color: #555555;

font-size: 18px;

line-height: 1.8;

}

Home html


WELCOME TO THE COURSE-INSTRUCTOR MANAGEMENT SYSTEM



This is the home page of our Course-Instructor Management System.

Use the navigation menu to manage instructors, courses, and their

assignments efficiently.



Instructor CSS
.section {

margin-bottom: 50px;

}

h2 {

font-size: 30px;

margin-bottom: 20px;

}

table {

width: 100%;

border-collapse: collapse;

box-shadow: 0 0 12px lightgray;

border-radius: 8px;

overflow: hidden;

}

thead {

background-color: #0d85f2;

color: white;

}

th,

td {

padding: 18px;

text-align: center;

font-size: 18px;

}

.edit-btn {

background-color: #28a745;

color: white;

border: none;

padding: 10px 18px;

margin-right: 8px;

border-radius: 5px;

}

.delete-btn {

background-color: #dc3545;

color: white;

border: none;

padding: 10px 18px;

border-radius: 5px;

}

.edit-card {

width: 500px;

margin: 40px auto;

padding: 30px;

box-shadow: 0 0 15px lightgray;

border-radius: 10px;

}

.edit-card h2 {

text-align: center;

color: #0d85f2;

}

label {

display: block;

font-weight: bold;

margin-top: 15px;

margin-bottom: 8px;

}

input {

width: 100%;

padding: 12px;

font-size: 16px;

}

.button-group {

margin-top: 25px;

}

.save-btn {

background-color: #0d85f2;

color: white;

border: none;

padding: 12px 22px;

margin-right: 8px;

border-radius: 5px;

}

.cancel-btn {

background-color: #6c757d;

color: white;

border: none;

padding: 12px 22px;

border-radius: 5px;

}

Instructor Html

Instructor List










<tr *ngFor="let instructor of instructors">








Instructor Name	Email	Hire Date	Actions
{{ instructor.Name }}	{{ instructor.Email }}	{{ instructor.HireDate }}	
<button class="edit-btn" (click)="onEditInstructor(instructor)">

Edit


<button class="delete-btn" (click)="onDeleteInstructor(instructor.InstructorId || 0)">

Delete


<div class="edit-card" *ngIf="editedInstructor">


Edit Instructor
Instructor Name:

<input type="text" [(ngModel)]="editedInstructor.Name">

Email:

<input type="email" [(ngModel)]="editedInstructor.Email">

Hire Date:

<input type="date" [(ngModel)]="editedInstructor.HireDate">


<button class="save-btn" (click)="onSaveEditedInstructor()">

Save


<button class="cancel-btn" (click)="onCancelEditInstructor()">

Cancel



Instructor Component
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { Instructor } from '../../models/instructor.model';

@Component({

selector: 'app-instructor',

templateUrl: './instructor.component.html',

styleUrls: ['./instructor.component.css']

})

export class InstructorComponent {

@Input() instructors: Instructor[] = [];

@Input() editedInstructor: Instructor | null = null;

@Output() editInstructorEvent = new EventEmitter();

@Output() saveEditedInstructorEvent = new EventEmitter();

@Output() cancelEditInstructorEvent = new EventEmitter();

@Output() deleteInstructorEvent = new EventEmitter();

onEditInstructor(instructor: Instructor): void {

this.editInstructorEvent.emit(instructor);

}

onSaveEditedInstructor(): void {

this.saveEditedInstructorEvent.emit();

}

onCancelEditInstructor(): void {

this.cancelEditInstructorEvent.emit();

}

onDeleteInstructor(instructorId: number): void {

this.deleteInstructorEvent.emit(instructorId);

}

}

Create-Instructor CSS
form {

width: 700px;

margin: 40px auto;

padding: 40px;

border-radius: 12px;

box-shadow: 0 0 15px lightgray;

}

h1 {

text-align: center;

color: #0d6efd;

font-size: 38px;

margin-bottom: 30px;

}

label {

display: block;

font-weight: bold;

font-size: 18px;

margin-top: 15px;

margin-bottom: 8px;

}

input {

width: 100%;

height: 42px;

padding: 8px;

font-size: 16px;

border: 1px solid #cccccc;

border-radius: 5px;

}

.error-message {

color: red;

margin-top: 8px;

margin-bottom: 12px;

font-style: italic;

}

button {

width: 100%;

height: 45px;

margin-top: 20px;

background-color: #0d6efd;

color: white;

border: none;

border-radius: 5px;

font-size: 18px;

}

button:disabled {

background-color: #cccccc;

}

Create-Instructor Html

CREATE NEW INSTRUCTOR

Instructor Name


<input type="text" id="instructorName" name="instructorName" [(ngModel)]="newInstructor.Name" required

#instructorNameRef="ngModel">

<div class="error-message" *ngIf="instructorNameRef.invalid && instructorNameRef.touched">

Instructor Name is required



Email


<input type="email" id="email" name="email" [(ngModel)]="newInstructor.Email" required #emailRef="ngModel">

<div class="error-message" *ngIf="emailRef.invalid && emailRef.touched">

Email is required



Hire Date


<input type="date" id="hireDate" name="hireDate" [(ngModel)]="newInstructor.HireDate" required

#hireDateRef="ngModel">

<div class="error-message" *ngIf="hireDateRef.invalid && hireDateRef.touched">

Hire Date is required


<div class="error-message" *ngIf="errorMessage">

{{ errorMessage }}


<button type="submit" id="submit" [disabled]="instructorForm.invalid">

CREATE


Create-Instructor Component
import { Component } from '@angular/core';

import { Router } from '@angular/router';

import { InstructorService } from '../../services/instructor.service';

import { Instructor } from '../../../models/instructor.model';

@Component({

selector: 'app-create-instructor',

templateUrl: './create-instructor.component.html',

styleUrls: ['./create-instructor.component.css']

})

export class CreateInstructorComponent {

newInstructor: Instructor = {

Name: '',

Email: '',

HireDate: ''

};

errorMessage: string = '';

constructor(

private instructorService: InstructorService,

private router: Router

) { }

createInstructor(): void {

this.errorMessage = '';

this.instructorService.createInstructor(this.newInstructor).subscribe({

next: () => {

this.router.navigate(['/admin']);

},

error: (error) => {

console.log('Create Instructor Error:', error);

this.errorMessage = 'Instructor not added';

}

});

}

}

Login CSS
.error-message {

color: red;

margin-top: 5px;

margin-bottom: 10px;

}

form {

width: 500px;

margin: auto;

}

input,

button {

width: 100%;

margin-bottom: 10px;

padding: 8px;

}

Login Html

Login
Username

<input type="text" id="username" name="username" [(ngModel)]="username" required #usernameRef="ngModel"

(input)="lastField = 'username'">

Password

<input type="password" id="password" name="password" [(ngModel)]="password" required #passwordRef="ngModel"

(input)="lastField = 'password'">


{{ getLoginErrorMessage() }}


<button id="submit" type="submit" [disabled]="loginForm.invalid">

Login


Login Component
import { Component } from '@angular/core';

import { Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

import { LoginModel } from '../../models/login-model.model';

@Component({

selector: 'app-login',

templateUrl: './login.component.html',

styleUrls: ['./login.component.css']

})

export class LoginComponent {

username: string = '';

password: string = '';

errorMessage: string = '';

lastField: string = '';

constructor(

private authService: AuthService,

private router: Router

) { }

login(): void {

this.errorMessage = '';

const loginData: LoginModel = {

Username: this.username,

Password: this.password

};

this.authService.login(loginData).subscribe({

next: (response) => {

const loggedInUser = response.user || response.User;

if (!loggedInUser) {

this.errorMessage = 'Invalid username or password';

return;

}

const userRole = loggedInUser.Role || loggedInUser.role;

if (userRole === 'Admin') {

this.router.navigate(['/admin']);

} else if (userRole === 'Organizer') {

this.router.navigate(['/organizer']);

} else {

this.router.navigate(['/home']);

}

},

error: () => {

this.errorMessage = 'Invalid username or password';

}

});

}

getLoginErrorMessage(): string {

if (this.lastField === 'password' && this.password === '') {

return 'Password is required';

}

if (this.lastField === 'username' && this.username === '') {

return 'Username is required';

}

return '';

}

}

Navbar CSS
.navbar {

background-color: #0d85f2;

color: white;

height: 80px;

display: flex;

align-items: center;

justify-content: space-between;

padding-left: 130px;

padding-right: 130px;

}

.navbar-title {

font-size: 28px;

font-weight: bold;

font-family: Georgia, serif;

}

.navbar-links {

display: flex;

align-items: center;

gap: 35px;

}

.navbar-links a {

color: white;

text-decoration: none;

font-size: 20px;

font-weight: bold;

font-family: Georgia, serif;

}

.navbar-links a:hover {

text-decoration: underline;

}

Navbar Html

Course-Instructor Management System




Home


<a routerLink="/organizer" *ngIf="authService.isOrganizer()">

Organizer

<a routerLink="/admin" *ngIf="authService.isAdmin()">

Admin

<a routerLink="/admin/createInstructor" *ngIf="authService.isAdmin()">

Create Instructor

<a routerLink="/admin/createCourse" *ngIf="authService.isAdmin()">

Create Course

<a routerLink="/signup" *ngIf="!authService.isLoggedIn()">

Register

<a routerLink="/login" *ngIf="!authService.isLoggedIn()">

Login

<a *ngIf="authService.isLoggedIn()" (click)="logout()">

Logout



Navbar Component
import { Component } from '@angular/core';

import { Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

@Component({

selector: 'app-navbar',

templateUrl: './navbar.component.html',

styleUrls: ['./navbar.component.css']

})

export class NavbarComponent {

constructor(

public authService: AuthService,

private router: Router

) { }

logout(): void {

this.authService.logout();

this.router.navigate(['/login']);

}

}

Organizer CSS
.organizer-container {

width: 82%;

margin: 50px auto;

padding: 40px;

border-radius: 12px;

box-shadow: 0 0 18px lightgray;

font-family: Georgia, serif;

}

h1 {

text-align: center;

color: #0d85f2;

font-size: 42px;

margin-bottom: 50px;

}

h2 {

color: #0d85f2;

font-size: 32px;

margin-bottom: 20px;

border-bottom: 2px solid #dddddd;

display: inline-block;

padding-bottom: 10px;

}

.section {

margin-bottom: 50px;

}

table {

width: 100%;

border-collapse: collapse;

box-shadow: 0 0 12px lightgray;

border-radius: 8px;

overflow: hidden;

margin-top: 20px;

}

thead {

background-color: #0d85f2;

color: white;

}

th,

td {

padding: 18px;

text-align: center;

font-size: 18px;

}

select {

width: 90%;

padding: 12px;

font-size: 16px;

border: 1px solid #cccccc;

border-radius: 5px;

}

.assign-btn,

.release-btn {

background-color: #28a745;

color: white;

border: none;

padding: 12px 22px;

border-radius: 5px;

font-size: 17px;

cursor: pointer;

}

.assign-btn:hover,

.release-btn:hover {

background-color: #218838;

}

.nested-table {

box-shadow: none;

margin: 0;

}

.nested-table td {

border: none;

padding: 14px;

}

#no_unassigned,

#no_instructors {

font-size: 18px;

margin-top: 25px;

}

Organizer html

INSTRUCTOR-COURSE ASSIGNMENT PANEL


Unassigned Courses
<p id="no_unassigned" *ngIf="unassignedCourses.length === 0">

No Unassigned Courses

<table *ngIf="unassignedCourses.length > 0">



TITLE

DESCRIPTION

DURATION

INSTRUCTOR

ACTION




<tr *ngFor="let course of unassignedCourses">

{{ course.Title }}

{{ course.Description }}

{{ course.Duration }} days


<select name="selectedInstructor{{ course.CourseId || 0 }}"

[(ngModel)]="selectedInstructorIds[course.CourseId || 0]">

<option [ngValue]="undefined">

Select Instructor

<option *ngFor="let instructor of instructors" [ngValue]="instructor.InstructorId || 0">

{{ instructor.Name }} ({{ instructor.Email }})




<button class="assign-btn"

(click)="assignCourseToInstructor(course, selectedInstructorIds[course.CourseId || 0])">

Assign to Instructor









Instructor List With Courses
<p id="no_instructors" *ngIf="instructors.length === 0">

No Instructors Available

<table *ngIf="instructors.length > 0">



INSTRUCTOR NAME

EMAIL

COURSES




<tr *ngFor="let instructor of instructors">

{{ instructor.Name }}

{{ instructor.Email }}




<tr *ngFor="let course of getCoursesByInstructor(instructor.InstructorId || 0)">







{{ course.Title }}	{{ course.Duration }} days	
<button class="release-btn"

(click)="releaseCourseFromInstructor(course, instructor.InstructorId || 0)">

Release Course








Organizer Component
import { Component, OnInit } from '@angular/core';

import { Course } from '../../models/course.model';

import { Instructor } from '../../models/instructor.model';

import { CourseService } from '../services/course.service';

import { InstructorService } from '../services/instructor.service';

@Component({

selector: 'app-organizer',

templateUrl: './organizer.component.html',

styleUrls: ['./organizer.component.css']

})

export class OrganizerComponent implements OnInit {

courses: Course[] = [];

instructors: Instructor[] = [];

unassignedCourses: Course[] = [];

selectedInstructorIds: { [courseId: number]: number } = {};

constructor(

private courseService: CourseService,

private instructorService: InstructorService

) { }

ngOnInit(): void {

this.getCourses();

this.getInstructors();

}

getCourses(): void {

this.courseService.getCourses().subscribe({

next: (data) => {

this.courses = data;

this.unassignedCourses = this.courses.filter(course =>

course.InstructorId === null ||

course.InstructorId === undefined

);

},

error: (error) => {

console.log(error);

}

});

}

getInstructors(): void {

this.instructorService.getInstructors().subscribe({

next: (data) => {

this.instructors = data;

},

error: (error) => {

console.log(error);

}

});

}

assignCourseToInstructor(course: Course, selectedInstructorId: number): void {

if (!selectedInstructorId || !course.CourseId) {

return;

}

const updatedCourse: Course = {

...course,

InstructorId: selectedInstructorId

};

this.courseService.updateCourse(course.CourseId, updatedCourse).subscribe({

next: () => {

this.getCourses();

this.getInstructors();

},

error: (error) => {

console.log(error);

}

});

}

releaseCourseFromInstructor(course: Course, selectedInstructorId: number): void {

if (!course.CourseId) {

return;

}

const updatedCourse: Course = {

...course,

InstructorId: null as any

};

this.courseService.updateCourse(course.CourseId, updatedCourse).subscribe({

next: () => {

this.getCourses();

this.getInstructors();

},

error: (error) => {

console.log(error);

}

});

}

getCoursesByInstructor(instructorId: number): Course[] {

return this.courses.filter(course => course.InstructorId === instructorId);

}

}

Registration CSS
.error-message,

.validation-message {

color: red;

margin-top: 8px;

margin-bottom: 10px;

font-style: italic;

}


form {

width: 500px;

margin: auto;

}


input,

select,

button {

width: 100%;

margin-bottom: 10px;

padding: 8px;

}


Registration Html

REGISTRATION
Username

<input type="text" id="username" name="username" [(ngModel)]="username" required #usernameRef="ngModel"

(input)="onFieldInput('username', $event)">

Password

<input type="password" id="password" name="password" [(ngModel)]="password" required #passwordRef="ngModel"

(input)="onFieldInput('password', $event)">

<div class="validation-message" *ngIf="password !== '' && !isPasswordStrong()">

Password must include at least one uppercase letter, one lowercase letter, one digit, and one special character


Confirm Password

<input type="password" id="confirmPassword" name="confirmPassword" [(ngModel)]="confirmPassword" required

#confirmPasswordRef="ngModel" (input)="onFieldInput('confirmPassword', $event)">

Role

<select id="role" name="role" [(ngModel)]="role" required #roleRef="ngModel" (change)="lastField = 'role'">

Select Role

Admin

Organizer



{{ getRegisterErrorMessage() }}


<button id="submit" type="submit" [disabled]="registerForm.invalid || password !== confirmPassword">

REGISTER


Registration Component
import { Component } from '@angular/core';

import { Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

import { User } from '../../models/user.model';

@Component({

selector: 'app-registration',

templateUrl: './registration.component.html',

styleUrls: ['./registration.component.css']

})

export class RegistrationComponent {

username: string = '';

password: string = '';

confirmPassword: string = '';

role: string = '';

errorMessage: string = '';

lastField: string = '';

passwordPattern: string = '.+';

constructor(

private authService: AuthService,

private router: Router

) { }

onFieldInput(fieldName: string, event: any): void {

this.lastField = fieldName;

if (fieldName === 'username') {

this.username = event.target.value;

}

if (fieldName === 'password') {

this.password = event.target.value;

}

if (fieldName === 'confirmPassword') {

this.confirmPassword = event.target.value;

}

}

register(): void {

this.errorMessage = '';

if (this.password !== this.confirmPassword) {

this.errorMessage = 'Passwords do not match';

return;

}

const newUser: User = {

Username: this.username,

Password: this.password,

Role: this.role

};

this.authService.register(newUser).subscribe({

next: () => {

this.router.navigate(['/login']);

},

error: () => {

this.errorMessage = 'Registration failed';

}

});

}

getRegisterErrorMessage(): string {

if (

this.password !== '' &&

this.confirmPassword !== '' &&

this.password !== this.confirmPassword

) {

return 'Passwords do not match';

}


if (this.lastField === 'username' && this.username === '') {

return 'Username is required';

}


if (this.lastField === 'password' && this.password === '') {

return 'Password is required';

}


if (this.lastField === 'confirmPassword' && this.confirmPassword === '') {

return 'Confirm Password is required';

}


return '';

}


isPasswordStrong(): boolean {

return (

/[A-Z]/.test(this.password) &&

/[a-z]/.test(this.password) &&

/\d/.test(this.password) &&

/[@$!%?&#]/.test(this.password)

);

}

}

========================================================================

Services
auth service

import { Injectable } from '@angular/core';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable, throwError } from 'rxjs';

import { catchError, tap } from 'rxjs/operators';

import { User } from '../../models/user.model';

import { LoginModel } from '../../models/login-model.model';

@Injectable({

providedIn: 'root'

})

export class AuthService {

public baseUrl = 'https://8080-ccdaacaf351882809adcdcbdcfaone.premiumproject.examly.io/api/users';

private isAuthenticatedSubject = new BehaviorSubject(this.isLoggedIn());

public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

constructor(private http: HttpClient) { }

register(newUser: User): Observable {

return this.http

.post(${this.baseUrl}/register, newUser)

.pipe(

catchError(this.handleError)

);

}

login(loginUser: LoginModel): Observable {

return this.http

.post(${this.baseUrl}/login, loginUser)

.pipe(

tap((response: any) => {

const loggedInUser = response.user || response.User;

if (loggedInUser) {

this.storeUserData(loggedInUser);

this.updateAuthenticationStatus(true);

}

}),

catchError(this.handleError)

);

}

logout(): void {

localStorage.removeItem('token');

localStorage.removeItem('role');

localStorage.removeItem('username');

this.updateAuthenticationStatus(false);

}

isLoggedIn(): boolean {

return localStorage.getItem('token') !== null;

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

private storeUserData(user: any): void {

localStorage.setItem('token', 'loggedIn');

if (user.Role) {

localStorage.setItem('role', user.Role);

}

if (user.role) {

localStorage.setItem('role', user.role);

}

if (user.Username) {

localStorage.setItem('username', user.Username);

}

if (user.username) {

localStorage.setItem('username', user.username);

}

}

private handleError(error: HttpErrorResponse): Observable {

console.log('API Error:', error);

return throwError(() => error);

}

}

2.Course service
import { Injectable } from '@angular/core';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';

import { Course } from '../../models/course.model';

@Injectable({

providedIn: 'root'

})

export class CourseService {

private baseUrl = 'https://8080-ccdaacaf351882809adcdcbdcfaone.premiumproject.examly.io/api/Course';

constructor(private http: HttpClient) { }

getCourses(): Observable<Course[]> {

return this.http

.get<Course[]>(${this.baseUrl}/GetCourses)

.pipe(catchError(this.handleError));

}

createCourse(course: Course): Observable {

return this.http

.post(${this.baseUrl}/PostCourse, course)

.pipe(catchError(this.handleError));

}

updateCourse(courseId: number, course: Course): Observable {

return this.http

.put(${this.baseUrl}/PutCourse/${courseId}, course)

.pipe(catchError(this.handleError));

}

deleteCourse(courseId: number): Observable {

return this.http

.delete(${this.baseUrl}/DeleteCourse/${courseId})

.pipe(catchError(this.handleError));

}

private handleError(error: HttpErrorResponse): Observable {

console.log('Course API Error:', error);

return throwError(() => error);

}

}

2***.Instructor service***
import { Injectable } from '@angular/core';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';

import { Instructor } from '../../models/instructor.model';

@Injectable({

providedIn: 'root'

})

export class InstructorService {

private baseUrl = 'https://8080-ccdaacaf351882809adcdcbdcfaone.premiumproject.examly.io/api/Instructor';

constructor(private http: HttpClient) { }

getInstructors(): Observable<Instructor[]> {

return this.http

.get<Instructor[]>(${this.baseUrl}/GetInstructors)

.pipe(catchError(this.handleError));

}

createInstructor(instructor: Instructor): Observable {

return this.http

.post(${this.baseUrl}/PostInstructor, instructor)

.pipe(catchError(this.handleError));

}

updateInstructor(instructorId: number, instructor: Instructor): Observable {

return this.http

.put(${this.baseUrl}/PutInstructor/${instructorId}, instructor)

.pipe(catchError(this.handleError));

}

deleteInstructor(instructorId: number): Observable {

return this.http

.delete(${this.baseUrl}/DeleteInstructor/${instructorId})

.pipe(catchError(this.handleError));

}

private handleError(error: HttpErrorResponse): Observable {

console.log('Instructor API Error:', error);

return throwError(() => error);

}

}

===========================================================================

Approuting Model
import { NgModule } from '@angular/core';

import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';

import { AdminComponent } from './admin/admin.component';

import { OrganizerComponent } from './organizer/organizer.component';

import { LoginComponent } from './login/login.component';

import { RegistrationComponent } from './registration/registration.component';

import { ErrorComponent } from './error/error.component';

import { CreateCourseComponent } from './course/create-course/create-course.component';

import { CreateInstructorComponent } from './instructor/create-instructor/create-instructor.component';

import { AuthGuard } from './authguard/auth.guard';

const routes: Routes = [

{

path: '',

component: HomeComponent

},

{

path: 'home',

component: HomeComponent

},

{

path: 'admin',

component: AdminComponent,

canActivate: [AuthGuard]

},

{

path: 'admin/createCourse',

component: CreateCourseComponent,

canActivate: [AuthGuard]

},

{

path: 'admin/createInstructor',

component: CreateInstructorComponent,

canActivate: [AuthGuard]

},

{

path: 'organizer',

component: OrganizerComponent,

canActivate: [AuthGuard]

},

{

path: 'login',

component: LoginComponent

},

{

path: 'signup',

component: RegistrationComponent

},

{

path: 'error',

component: ErrorComponent

},

{

path: '**',

redirectTo: 'error'

}

];

@NgModule({

imports: [RouterModule.forRoot(routes)],

exports: [RouterModule]

})

export class AppRoutingModule { }

App-module
import { NgModule } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';

import { FormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';

import { RegistrationComponent } from './registration/registration.component';

import { LoginComponent } from './login/login.component';

import { NavbarComponent } from './navbar/navbar.component';

import { AdminComponent } from './admin/admin.component';

import { OrganizerComponent } from './organizer/organizer.component';

import { CreateCourseComponent } from './course/create-course/create-course.component';

import { CreateInstructorComponent } from './instructor/create-instructor/create-instructor.component';

import { HomeComponent } from './home/home.component';

import { ErrorComponent } from './error/error.component';

import { CourseComponent } from './course/course.component';

import { InstructorComponent } from './instructor/instructor.component';

@NgModule({

declarations: [

AppComponent,

RegistrationComponent,

LoginComponent,

NavbarComponent,

AdminComponent,

OrganizerComponent,

HomeComponent,

ErrorComponent,

CreateCourseComponent,

CreateInstructorComponent,

CourseComponent,

InstructorComponent

],

imports: [

BrowserModule,

AppRoutingModule,

FormsModule,

HttpClientModule

],

providers: [],

bootstrap: [AppComponent]

})

export class AppModule { }

========================================================================

Models
Course

import { Instructor } from './instructor.model';

export interface Course {

CourseId?: number;

Title: string;

Description: string;

Duration: number;

InstructorId?: number;

Instructor?: Instructor;

}

2. Instructor
import { Course } from './course.model';

export interface Instructor {

InstructorId?: number;

Name: string;

Email: string;

HireDate: Date | string;

Courses?: Course[];

}

3. login model
export interface LoginModel {

Username: string;

Password: string;

}


4. user model
export interface User {

Id?: number;

Username: string;

Password: string;

Role: string;

}


===========================================================================

dotnetapp Program.cs
using dotnetapp.Models;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()

.AddJsonOptions(options =>

{

options.JsonSerializerOptions.PropertyNamingPolicy = null;

});

// CORS configuration

builder.Services.AddCors(options =>

{

options.AddPolicy("AllowAngular",

policy =>

{

policy.AllowAnyOrigin()

.AllowAnyHeader()

.AllowAnyMethod();

});

});

// Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Database connection

builder.Services.AddDbContext(o =>o.UseSqlServer("User Id=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=false;Encrypt=false"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

