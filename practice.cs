9 July
Session 1 cod 1

auth.guard.ts



import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router
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

  canActivate(): boolean {

    if (this.authService.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['/error']);
    return false;
  }
}

adminpage.component.ts


import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-adminpage',
  templateUrl: './adminpage.component.html',
  styleUrls: ['./adminpage.component.css']
})
export class AdminpageComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}

adminpage.component.html

<h1>Admin Page</h1>

<button (click)="logout()">
  Logout
</button>

error.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {

}

error.component.html

<h1>Unauthorized Access</h1>*
<p>You are not authorized to view*this page.</p>

<a routerLink="/lo*in">
  Go to Login
</a>

login.component.ts

import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from 'src/app/services/auth.service';
import { Login } from 'src/app/models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginData: Login = {
    username: '',
    password: ''
  };

  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {

    this.authService.login(this.loginData)
      .subscribe(res => {

    if (res) {

    localStorage.setItem(
            'isLoggedIn',
            'true'
          );

    this.router.navigate(['/admin']);

    } else {

    this.errorMessage =
            'Invalid username or password';
        }

    });
  }
}

login.component.html



<h1>Login</h1>

<input
  type="text"
  [(ngModel)]="loginData.username"
  placeholder="Username">

<input
  type="password"
  [(ngModel)]="loginData.password"
  placeholder="Password">

<button (click)="login()">
  Login
</button>

<p *ngIf="errorMessage" style="color:red">
  {{ errorMessage }}
</p>



models/login.model.ts

export interface Login {
    username: string;
    password: string;
  }
  
auth/auth.service.ts



import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Login } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  login(loginData: Login): Observable<boolean> {

    if (
      loginData.username === 'admin' &&
      loginData.password === 'password'
    ) {
      return of(true);
    }

    return of(false);
  }

  isAuthenticated(): boolean {
    return localStorage.getItem('isLoggedIn') === 'true';
  }

  logout(): void {
    localStorage.removeItem('isLoggedIn');
  }
}

app.module.ts


import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LoginComponent } from './components/login/login.component';
import { AdminpageComponent } from './components/adminpage/adminpage.component';
import { ErrorComponent } from './components/error/error.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AdminpageComponent,
    ErrorComponent
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

app.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
}


app-routing.module.ts


import { NgModule } from '@angular/core';
import {

RouterModule,
  Routes
  } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { AdminpageComponent } from './components/adminpage/adminpage.component';
import { ErrorComponent } from './components/error/error.component';
import { AuthGuard } from './authguard/auth.guard';

const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'admin',
    component: AdminpageComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'error',
    component: ErrorComponent
  },

  {
    path: '**',
    redirectTo: 'login'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


=================

9 July Session 1 Cod 2

auth.guard.ts



import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router
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

  canActivate(): boolean {

    if (this.authService.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['/error']);
    return false;
  }
}


error.component.html

<h1>Unauthorized Access</h1>

<p>
  You are not authorized to view this page.
</p>

<a routerLink="/login">
  Go to Login
</a>

error.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {

}

login.component.ts



import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginData: Login = {
    username: '',
    password: ''
  };

  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {

    this.authService
      .login(this.loginData)
      .subscribe((res: any) => {

    if (res) {

    localStorage.setItem(
            'isLoggedIn',
            'true'
          );

    this.router.navigate(['/user']);

    } else {

    this.errorMessage =
            'Invalid username or password';
        }
      });
  }
}


login.component.html





<h1>Login</h1>

<input
  type="text"
  [(ngModel)]="loginData.username"
  placeholder="Username">

<input
  type="password"
  [(ngModel)]="loginData.password"
  placeholder="Password">

<button (click)="login()">
  Login
`</button>`

<p *ngIf="errorMessage" style="color:red">
  {{ errorMessage }}
</p>



userpage.component.ts

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-userpage',
  templateUrl: './userpage.component.html',
  styleUrls: ['./userpage.component.css']
})
export class UserpageComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);
  }
}


userpage.component.html

<h1>Welcome User!</h1>

<button (click)="logout()">
  Logout
`</button>`

models/login.model.ts

export interface Login {
    username: string;
    password: string;
  }
  
services/auth.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Login } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  apiUrl =
    'https://8080--premiumproject.examly.io/api/login';

  constructor(private http: HttpClient) { }

  login(loginData: Login): Observable<any> {
    return this.http.post<any>(
      this.apiUrl,
      loginData
    );
  }

  isAuthenticated(): boolean {
    return localStorage.getItem('isLoggedIn') === 'true';
  }

  logout(): void {
    localStorage.removeItem('isLoggedIn');
  }
}

app-routing.module.ts

import { NgModule } from '@angular/core';
import {
  RouterModule,
  Routes
} from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { UserpageComponent } from './components/userpage/userpage.component';
import { ErrorComponent } from './components/error/error.component';
import { AuthGuard } from './authguard/auth.guard';

const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'user',
    component: UserpageComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'error',
    component: ErrorComponent
  },

  {
    path: '**',
    redirectTo: 'login'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

app.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
}


app.module.ts


import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { LoginComponent } from './components/login/login.component';
import { UserpageComponent } from './components/userpage/userpage.component';
import { ErrorComponent } from './components/error/error.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserpageComponent,
    ErrorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,      // IMPORTANT
    HttpClientModule  // IMPORTANT
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

=================


9 July
Session 2 Cod 1

authguard/auth.guard.ts

import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router
} from '@angular/router';

import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(): boolean {

    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }
}

components/dashboard/dashboard.component.ts

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);
  }
}


dashboard.component.html



<h1>Dashboard</h1>

<p>Token stored successfully</p>

<button (click)="logout()">
  Logout
`</button>`

login.component.ts


import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  username = '';
  password = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {

    this.authService
      .login(this.username, this.password)
      .subscribe(
        (res: any) => {

    const token =
            res.token || 'sample-token';

    this.authService.storeToken(token);

    this.router.navigate(['/dashboard']);
        },
        () => {
          this.errorMessage =
            'Invalid username or password';
        }
      );
  }
}


login.component.html

<p>login works!</p>


<h1>Login</h1>

<input
  type="text"
  [(ngModel)]="username"
  placeholder="Username">

<input
  type="password"
  [(ngModel)]="password"
  placeholder="Password">

<button (click)="login()">
  Login
`</button>`

<p *ngIf="errorMessage">
  {{ errorMessage }}
</p>


models/login.model.ts

export class Login {
    username: string = '';
    password: string = '';
  }
  
services/auth.service.ts


import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtService } from './jwt.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl =
    'https://8080--premiumproject.examly.io/api/login';

  constructor(
    private http: HttpClient,
    private jwtService: JwtService
  ) { }

  login(
    username: string,
    password: string
  ): Observable<any> {

    return this.http.post<any>(
      this.apiUrl,
      {
        username: username,
        password: password
      }
    );
  }

  storeToken(token: string): void {
    this.jwtService.saveToken(token);
  }

  logout(): void {
    this.jwtService.destroyToken();
  }

  isLoggedIn(): boolean {
    return this.jwtService.isLoggedIn();
  }
}


services/jwt.service.ts

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor() { }

  saveToken(token: string): void {
    localStorage.setItem('jwt_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  destroyToken(): void {
    localStorage.removeItem('jwt_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}

app-routing.module.ts

import { NgModule } from '@angular/core';
import {
  RouterModule,
  Routes
} from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './components/authguard/auth.guard';

const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },

  {
    path: '**',
    redirectTo: 'login'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


app.component.html



<router-outlet></router-outlet>


app.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
}


app.module.ts


import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent
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


=================================

Session 2 Cod 2

src/app/services/jwt.service.ts

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  saveToken(token: string): void {
    localStorage.setItem('jwt_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  destroyToken(): void {
    localStorage.removeItem('jwt_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}

src/app/services/auth.service.ts

import { Injectable } from '@angular/core';
import { JwtService } from './jwt.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtService: JwtService) {}

  storeToken(token: string): void {
    this.jwtService.saveToken(token);
  }

  logout(): void {
    this.jwtService.destroyToken();
  }

  isLoggedIn(): boolean {
    return this.jwtService.isLoggedIn();
  }

  login(username: string, password: string): any {
    return {
      subscribe: (fn: any) => fn({ token: 'token' })
    };
  }
}

src/app/components/authguard/auth.guard.ts

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {

    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }
}

src/app/components/login/login.component.ts

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  username = '';
  password = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    this.authService
      .login(this.username, this.password)
      .subscribe((res: any) => {

        this.authService.storeToken(res.token);
        this.router.navigate(['/profile']);

      });
  }
}

src/app/components/login/login.component.html

<input [(ngModel)]="username">
<input [(ngModel)]="password">
<button (click)="login()">Login</button>

src/app/components/profile/profile.component.ts

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

src/app/components/profile/profile.component.html

<button (click)="logout()">Logout</button>

src/app/app.module.ts

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { ProfileComponent } from './components/profile/profile.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

src/app/app.component.html

Empty file

================================

session 3 cod 1

auth.service.ts



import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public apiUrl = 'https://8080---premiumproject.examly.io/api/login';

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {

    return this.http.post<any>(this.apiUrl, {
      username,
      password
    }).pipe(
      tap((res: any) => {
        if (res && res.token) {
          localStorage.setItem('token', res.token);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
  }
}


auth.interceptor.ts


import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler
} from '@angular/common/http';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler) {

    const token = localStorage.getItem('token');

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization:`Bearer ${token}`
        }
      });
    }

    return next.handle(req);
  }
}

login.component.ts



import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  username = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    this.authService
      .login(this.username, this.password)
      .subscribe(() => {

    this.router.navigate(['/dashboard']);

    });

  }

}


login.component.html


<button (click)="login()">Login `</button>`

dashboard.component.ts


import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  logout(): void {

    this.authService.logout();
    this.router.navigate(['/login']);

  }

}


dashboard.component.html

<button (click)="logout()">Logout `</button>`

app.module.ts

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


app.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
}


app.component.html

<p></p>

======================

Session 3 cod 2

login.component.ts

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  username = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    this.authService
      .login(this.username, this.password)
      .subscribe(() => {

    this.router.navigate(['/profile']);

    });
  }

}


login.component.html


<button (click)="login()">Login `</button>`

profile.component.ts


import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent {

  profileData: any;

  constructor(private authService: AuthService) {}

  loadProfile(): void {

    this.authService
      .getProfile()
      .subscribe(data => {

    this.profileData = data;

    });
  }

}


profile.component.html

<button (click)="loadProfile()">Load `</button>`


interceptors

import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpResponse
} from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class LoggingInterceptor implements HttpInterceptor {

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    console.log('Request URL:', req.url);
    console.log('Request Method:', req.method);

    return next.handle(req).pipe(
      tap((event: any) => {
        if (event instanceof HttpResponse) {
          console.log('Response Status:', event.status);
        }
      })
    );
  }
}

auth.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  apiUrl = 'https://8080---premiumproject.examly.io/api';

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, {
      username,
      password
    });
  }

  getProfile() {
    return this.http.get(`${this.apiUrl}/profile`);
  }

}

app.component.html

<p></p>


app.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularapp';
}


app.module.ts

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { LoggingInterceptor } from './interceptors/logging.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoggingInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }






























