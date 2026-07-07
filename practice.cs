
 
registration-form.component.ts
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
 
@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent {
 
  isSubmitted: boolean = false;
  passwordMismatch: boolean = false;
 
  onSubmit(form: NgForm): void {
 
    if (form.value.password !== form.value.confirmPassword) {
      this.passwordMismatch = true;
      this.isSubmitted = false;
      return;
    }
 
    this.passwordMismatch = false;
 
    if (form.valid) {
      this.isSubmitted = true;
      form.resetForm();
    }
  }
}
 
 
registration-form.component.html
 
<h1>Registration Form</h1>
 
<form #registrationForm="ngForm"
      (ngSubmit)="onSubmit(registrationForm)">
 
  <label for="name">Name</label>
  <input
    type="text"
    id="name"
    name="name"
    ngModel
    required>
 
  <label for="email">Email</label>
  <input
    type="email"
    id="email"
    name="email"
    ngModel
    required
    email>
 
  <label for="password">Password</label>
  <input
    type="password"
    id="password"
    name="password"
    ngModel
    required>
 
  <label for="confirmPassword">Confirm Password</label>
  <input
    type="password"
    id="confirmPassword"
    name="confirmPassword"
    ngModel
    required>
 
<div *ngIf="passwordMismatch" class="error-message">
    Passwords do not match
  </div>
 
  <button
    type="submit"
    [disabled]="registrationForm.invalid">
    Register
  </button>
 
</form>
 
<div *ngIf="isSubmitted" class="success-message">
  Registration successful!
</div>
 
 
app.module.ts
 
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
 
@NgModule({
  declarations: [
    AppComponent,
    RegistrationFormComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.component.html
<app-registration-form></app-registration-form>
 
