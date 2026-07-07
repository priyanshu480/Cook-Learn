contact-form.component.ts
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
 
@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent {
 
  contactForm: FormGroup;
 
  constructor(private fb: FormBuilder) {
 
    this.contactForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
 
    address: this.fb.group({
        street: [''],
        city: [''],
        state: [''],
        postalCode: ['']
      }),
 
    subjectDetails: this.fb.group(
        {
          subject: [''],
          message: ['']
        },
        {
          validators: this.subjectMessageValidator
        }
      )
    });
  }
 
  subjectMessageValidator(
    group: AbstractControl
  ): ValidationErrors | null {
 
    const subject = group.get('subject')?.value;
    const message = group.get('message')?.value;
 
    if ((subject && !message) || (!subject && message)) {
      return { subjectMessageRequired: true };
    }
 
    return null;
  }
 
  onSubmit(): void {
 
    if (this.contactForm.valid) {
 
    console.log(this.contactForm.value);
 
    this.contactForm.reset();
 
    } else {
 
    console.log('Form Invalid');
 
    this.contactForm.markAllAsTouched();
    }
  }
 
  get firstName() {
    return this.contactForm.get('firstName');
  }
 
  get lastName() {
    return this.contactForm.get('lastName');
  }
 
  get email() {
    return this.contactForm.get('email');
  }
}
 
 
contact-form.component.html
 
 
<h1>Contact Form</h1>
 
<form
  [formGroup]="contactForm"
  (ngSubmit)="onSubmit()">
 
<div>
    <label>First Name</label>
 
    `<input
      type="text"
      formControlName="firstName">`
 
    <div
      *ngIf="firstName?.touched && firstName?.errors?.['required']">
      First Name is required`</div>`
 
    <div
      *ngIf="firstName?.touched && firstName?.errors?.['minlength']">
      Minimum 2 characters required`</div>`
 
</div>
 
<br>
 
<div>
    <label>Last Name</label>
 
    `<input
      type="text"
      formControlName="lastName">`
 
    <div
      *ngIf="lastName?.touched && lastName?.errors?.['required']">
      Last Name is required`</div>`
 
    <div
      *ngIf="lastName?.touched && lastName?.errors?.['minlength']">
      Minimum 2 characters required`</div>`
 
</div>
 
<br>
 
<div>
    <label>Email</label>
 
    `<input
      type="email"
      formControlName="email">`
 
    <div
      *ngIf="email?.touched && email?.errors?.['required']">
      Email is required`</div>`
 
    <div
      *ngIf="email?.touched && email?.errors?.['email']">
      Invalid email format`</div>`
 
</div>
 
<br>
 
<div formGroupName="address">
 
    `<h3>`Address`</h3>`
 
    `<input
      type="text"
      formControlName="street"
      placeholder="Street">`
 
    `<input
      type="text"
      formControlName="city"
      placeholder="City">`
 
    `<input
      type="text"
      formControlName="state"
      placeholder="State">`
 
    `<input
      type="text"
      formControlName="postalCode"
      placeholder="Postal Code">`
 
</div>
 
<br>
 
<div formGroupName="subjectDetails">
 
    `<h3>`Subject Details`</h3>`
 
    `<input
      type="text"
      formControlName="subject"
      placeholder="Subject">`
 
    `<textarea
      formControlName="message"
      placeholder="Message"></textarea>`
 
    <div
      *ngIf="contactForm.get('subjectDetails')?.errors?.['subjectMessageRequired']">
      Subject and Message must both be filled.`</div>`
 
</div>
 
<br>
 
  <button
    type="submit"
    [disabled]="contactForm.invalid">
    Submit
  `</button>`
 
</form>
 
 
app.module.ts
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
 
import { AppComponent } from './app.component';
import { ContactFormComponent } from './contact-form/contact-form.component';
 
@NgModule({
  declarations: [
    AppComponent,
    ContactFormComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
app.comonent.html
<app-contact-form></app-contact-form>
