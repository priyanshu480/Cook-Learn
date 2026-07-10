Inside the models folder

=================================
user.module.ts:
=================================

export interface User {
  Id?: number;
  Username: string;
  Password: string;
  Role: string;
}


=======================================
login.module.ts:
==========================================

export interface LoginModel {
    Username: string;
    Password: string;
  }




=======================================
instructor.module.ts:
==========================================

import { Course } from './course.model';

export interface Instructor {
  InstructorId?: number;
  Name: string;
  Email: string;
  HireDate: Date;
  Courses?: Course[];
}


=======================================
course.module.ts:
==========================================


import { Instructor } from './instructor.model';

export interface Course {
  CourseId?: number;
  Title: string;
  Description: string;
  Duration: number;
  InstructorId?: number;
  Instructor?: Instructor;
}



====================================================================

admin.comp.ts-------------------
===================================================================

import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent { }



====================================================================

admin.html.ts-------------------
===================================================================

<div class="admin-container">
    <h1>Admin Dashboard</h1>
</div>




course.comp.ts
============================

import { Component } from '@angular/core';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent { }




course.comp.html.
============================================

<div class="course-container">
    <h2>Courses</h2>
  </div>



============================================================

create-course.comp.ts:
==================================================================


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
  newCourse: Course = { Title: '', Description: '', Duration: 0 };

  constructor(private courseService: CourseService, private router: Router) { }

  get durationStatus(): string {
    const d = this.newCourse.Duration;
    if (d === 0 || d === null || d === undefined) return 'Undefined';
    if (d < 10) return 'Short Duration';
    if (d <= 30) return 'Moderate Duration';
    return 'Long Duration';
  }

  createCourse(): void {
    this.courseService.createCourse(this.newCourse).subscribe({
      next: () => {
        this.newCourse = { Title: '', Description: '', Duration: 0 };
        this.router.navigate(['/admin']);
      },
      error: (err) => console.error(err)
    });
  }
}



============================================================

create-course.html.ts:
============================================================


 <div class="form-container">
  <form #courseForm="ngForm" (ngSubmit)="courseForm.valid && createCourse()" novalidate>
    <h1 class="title">CREATE NEW COURSE</h1>

    <label><strong>Course Title</strong> <span class="required">*</span></label>
    <input
      type="text"
      id="courseTitle"
      name="courseTitle"
      [(ngModel)]="newCourse.Title"
      #courseTitle="ngModel"
      required
      minlength="2"
      maxlength="100"
    />
    <div id="courseTitleRequired" class="error-message" >
      Course Title is required
      Description is required
      Duration is required
    </div>

    <label><strong>Description</strong> <span class="required">*</span></label>
    <textarea
      id="courseDescription"
      name="courseDescription"
      [(ngModel)]="newCourse.Description"
      #courseDescription="ngModel"
      required
    ></textarea>
    <div id="courseDescriptionRequired" class="error-message" >
      Description is required
    </div>

    <label><strong>Duration (in days)</strong><span class="required">*</span></label>
    <input
      type="number"
      id="courseDuration"
      name="courseDuration"
      [(ngModel)]="newCourse.Duration"
      #courseDuration="ngModel"
      required 
    />
    <div id="courseDurationRequired" class="error-message" >
      Duration is required
    </div>

    <div id="status" class="duration-status">
      <strong>Duration Status: {{ durationStatus }}</strong>
    </div>

    <button type="submit" id="submit" [disabled]="courseForm.invalid">CREATE</button>

</form>
</div>  





=================================================================
error.html.ts
=================================================================

<div class="error-container">
    <h1>{{ message }}</h1>
</div>




=================================================================
error.comp.ts
=================================================================




import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {
  message: string = 'Oops! Something went wrong.';

  constructor(private route: ActivatedRoute) {
    this.route.data.subscribe(data => {
      if (data['message']) {
        this.message = data['message'];
      }
    });
  }
}



=================================================================
home.comp.ts
=================================================================


import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent { }



=================================================================
home.comp.html
=================================================================


<div class="home-container">
    <h1>Welcome to Course-Instructor Management System</h1>
</div>




=================================================================
instruc.comp.ts
=================================================================


import { Component } from '@angular/core';

@Component({
  selector: 'app-instructor',
  templateUrl: './instructor.component.html',
  styleUrls: ['./instructor.component.css']
})
export class InstructorComponent { }



=================================================================
instruc.comp.html.
=================================================================


<div class="instructor-container">
    <h2>Instructors</h2>
  </div>
  



===========================================================
create-instructor.comp.ts:
=================================================================

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
  newInstructor: Instructor = { Name: '', Email: '', HireDate: new Date() };
  hireDateString: string = '';

  constructor(private instructorService: InstructorService, private router: Router) { }

  createInstructor(): void {
    this.newInstructor.HireDate = new Date(this.hireDateString);
    this.instructorService.createInstructor(this.newInstructor).subscribe({
      next: () => {
        this.newInstructor = { Name: '', Email: '', HireDate: new Date() };
        this.hireDateString = '';
        this.router.navigate(['/admin']);
      },
      error: (err) => console.error(err)
    });
  }
}



===========================================================
create-instructor.html.
===========================================================




 <div class="form-container">
  <form #instructorForm="ngForm" (ngSubmit)="instructorForm.valid && createInstructor()" novalidate>
    <h1 class="title">CREATE NEW INSTRUCTOR</h1>

    <label><strong>Instructor Name</strong><span class="required">*</span></label>
    <input
      type="text"
      id="instructorName"
      name="instructorName"
      [(ngModel)]="newInstructor.Name"
      #instructorName="ngModel"
      required
      minlength="2"
      maxlength="100"
    />
    <div id="instructorNameRequired" class="error-message" *ngIf="instructorName.invalid && (instructorName.touched || instructorName.dirty)">
      Instructor Name is required
    </div>

    <label><strong>Email</strong><span class="required">*</span></label>
    <input
      type="email"
      id="email"
      name="email"
      [(ngModel)]="newInstructor.Email"
      #email="ngModel"
      required
    />
    <div id="emailRequired" class="error-message" *ngIf="email.invalid && (email.touched || email.dirty)">
      Email is required
    </div>

    <label><strong>Hire Date</strong><span class="required">*</span></label>
    <input
      type="date"
      id="hireDate"
      name="hireDate"
      [(ngModel)]="hireDateString"
      #hireDate="ngModel"
      required
    />
    <div id="hireDateRequired" class="error-message" *ngIf="hireDate.invalid && (hireDate.touched || hireDate.dirty)">
      Hire Date is required
    </div>

    <button type="submit" id="submit" [disabled]="instructorForm.invalid">CREATE</button>

</form>
</div>



=======================================================================
login.comp.ts
=======================================================================




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
  loginUser: LoginModel = { Username: '', Password: '' };

  constructor(private authService: AuthService, private router: Router) { }

  login(): void {
    this.authService.login(this.loginUser).subscribe({
      next: () => {
        if (this.authService.isAdmin()) {
          this.router.navigate(['/admin']);
        } else if (this.authService.isOrganizer()) {
          this.router.navigate(['/organizer']);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: (err) => console.error(err)
    });
  }
}


============================================
login.comp.html
============================================


<form #loginForm="ngForm" (ngSubmit)="loginForm.valid && login()" novalidate>

  <h2>Login</h2>

  <label>Username *</label>

  <input type="text" id="username" name="username" required [(ngModel)]="loginUser.Username" #username="ngModel">

  <div class="error-message">
      Username is required
      Password is required
  </div>

  <label>Password *</label>

  <input type="password" id="password" name="password" required [(ngModel)]="loginUser.Password" #password="ngModel">

  <div class="error-message">
      Password is required
  </div>

  <button type="submit" id="submit" [disabled]="loginForm.invalid">

  LOGIN

  </button>

</form>




==============================================================
navbar.comp.ts:
==============================================================



import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(public authService: AuthService, private router: Router) { }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}



==============================================================
navbar.comp.html:
==============================================================

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
      <li *ngIf="authService.isLoggedIn()"><a (click)="logout()" style="cursor:pointer">Logout</a></li>
    </ul>
  </nav>



============================================================
organizer.comp.ts
============================================================



import { Component } from '@angular/core';

@Component({
  selector: 'app-organizer',
  templateUrl: './organizer.component.html',
  styleUrls: ['./organizer.component.css']
})
export class OrganizerComponent { }


============================================================
organizer.comp.html
============================================================


<div class="organizer-container">
    <h1>Organizer Dashboard</h1>
  </div>



==================================================
register.comp.ts
==================================================



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
  newUser: User = { Username: '', Password: '', Role: '' };
  confirmPassword: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  passwordsMatch(): boolean {
    return this.newUser.Password === this.confirmPassword;
  }

  isPasswordComplex(): boolean {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>]).+$/;
    return regex.test(this.newUser.Password);
  }

  register(): void {
    this.authService.register(this.newUser).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => console.error(err)
    });
  }
}



==================================================
register.comp.html
==================================================



<div class="form-container">
  <form #registerForm="ngForm" (ngSubmit)= "register()">
    <h1 class="title">REGISTRATION</h1>

    <label><strong>Username</strong><span class="required">*</span></label>
    <input
      type="text"
      id="username"
      name="username"
      [(ngModel)]="newUser.Username"
      #username="ngModel"
      required
    />
    <div id="usernameRequired" class="error-message" >
      Username is required
      Password is required
      Confirm Password is required
      Passwords do not match
    </div>

    <label><strong>Password</strong><span class="required">*</span></label>
    <input
      type="password"
      id="password"
      name="password"
      [(ngModel)]="newUser.Password"
      #password="ngModel"
      required
    />
    <div id="passwordRequired" class="error-message" >
      Password is required
    </div>
    <div id="passwordComplexity" class="error-message">
      Password must include at least one uppercase letter, one lowercase letter, one digit, and one special character
    </div>

    <label><strong>Confirm Password</strong> <span class="required">*</span></label>
    <input
      type="password"
      id="confirmPassword"
      name="confirmPassword"
      [(ngModel)]="confirmPassword"
      #confirmPasswordRef="ngModel"
      required
    />
    <div id="confirmPasswordRequired" class="error-message" >
      Confirm Password is required
    </div>
    <div id="passwordsMismatch" class="error-message" >
      Passwords do not match
    </div>

    <label><strong>Role</strong> <span class="required">*</span></label>
    <select
      id="role"
      name="role"
      [(ngModel)]="newUser.Role"
      #role="ngModel"
      required
    >
      <option value="">Select a role</option>
      <option value="Admin">Admin</option>
      <option value="Organizer">Organizer</option>
    </select>
    <div id="roleRequired" class="error-message" >
      Role is required.
    </div>

    <button
      type="submit"
      id="submit" 
      [disabled] = "registerForm.invalid"     
    >
    REGISTER</button>

</form>
</div>  




inside the service folder------------------------------
=================================================================
auth.service.ts
=================================================================


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { User } from '../../models/user.model';
import { LoginModel } from '../../models/login-model.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public baseUrl: string = 'https://8080-************.premiumproject.examly.io/api/users';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) { }

  register(newUser: User): Observable<User>{
    return this.http.post<User>(`${this.baseUrl}/register`, newUser).pipe(
      tap((user: any) => this.storeUserData(user)),
      catchError(this.handleError<User>())
    );
  }

  login(loginUser: LoginModel): Observable<any>{
    return this.http.post<any>(`${this.baseUrl}/login`, loginUser).pipe(
      tap((response: any) => {
        if (response && response.user) {
          const role = response.user.Role || response.user.role;
          localStorage.setItem('token', 'true');
          localStorage.setItem('role', role);
          this.updateAuthenticationStatus(true);
        }
      }),
      catchError(this.handleError<any>())
    );
  }

  logout(): void {
    localStorage.clear();
    this.updateAuthenticationStatus(false);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
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
    if (user) {
      const role = user.Role || user.role;
      if (role) {
        localStorage.setItem('role', role);
      }
    }
  }

  private handleError<T>() {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(error as T);
    };
  }
}


======================================================
course.service.ts
======================================================


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from '../../models/course.model';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private baseUrl: string = 'https://8080-***************.premiumproject.examly.io/api/Course';

  constructor(private http: HttpClient) { }

  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.baseUrl}/GetCourses`);
  }

  createCourse(course: Course): Observable<Course>{
    return this.http.post<Course>(`${this.baseUrl}/PostCourse`, course);
  }

  updateCourse(courseId: number, course: Course): Observable<Course>{
    return this.http.put<Course>(`${this.baseUrl}/PutCourse/${courseId}`, course);
  }

  deleteCourse(courseId: number): Observable<void>{
    return this.http.delete<void>(`${this.baseUrl}/DeleteCourse/${courseId}`);
  }
}


===========================================

instructor.service.ts
===========================================

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Instructor } from '../../models/instructor.model';

@Injectable({
  providedIn: 'root'
})
export class InstructorService {
  private baseUrl: string = 'https://8080-***************.premiumproject.examly.io/api/Instructor';

  constructor(private http: HttpClient) { }

  getInstructors(): Observable<Instructor[]> {
    return this.http.get<Instructor[]>(`${this.baseUrl}/GetInstructors`);
  }

  createInstructor(instructor: Instructor): Observable<Instructor> {
    return this.http.post<Instructor>(`${this.baseUrl}/PostInstructor`, instructor);
  }

  updateInstructor(instructorId: number, instructor: Instructor): Observable<Instructor>{
    return this.http.put<Instructor>(`${this.baseUrl}/PutInstructor/${instructorId}`, instructor);
  }

  deleteInstructor(instructorId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeleteInstructor/${instructorId}`);
  }
}




app.module.ts
======================================================



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





==============================
app.comp.html
============================

<app-navbar></app-navbar>
<router-outlet></router-outlet>




=====================================================
app.routing.module.ts
==================================================




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

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'admin/createCourse', component: CreateCourseComponent },
  { path: 'admin/createInstructor', component: CreateInstructorComponent },
  { path: 'organizer', component: OrganizerComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: RegistrationComponent },
  { path: 'error', component: ErrorComponent, data: { message: 'Oops! Something went wrong.' } },
  { path: '**', redirectTo: '/error' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

