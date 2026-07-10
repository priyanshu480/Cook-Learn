--- Delete-confirmed.html 

<h2>Delete Confirmation</h2>

<ul>
    <li>Event Name: {{event.eventName}}</li>
    <li>Description: {{event.eventDescription}}</li>
    <li>Date: {{event.eventDate}}</li>
    <li>Time: {{event.eventTime}}</li>
    <li>Location: {{event.eventLocation}}</li>
    <li>Organizer: {{event.eventOrganizer}}</li>
</ul>
<button class="confirm-button" type="button" (click)="confirmDelete(event.eventId)">Confirm Delete</button>
<button class="cancel-button" type="button" (click)="cancelDelete()">Cancel</button>


----- Delete-confirm.ts 

import { Component, OnInit } from '@angular/core';
import { Event } from '../models/event.model';
import { EventService } from '../services/event.service';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-delete-confirm',
  templateUrl: './delete-confirm.component.html',
  styleUrls: ['./delete-confirm.component.css']
})
export class DeleteConfirmComponent implements OnInit {
  event!: Event;
  eventId! : number;

  constructor(private eventService: EventService, private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
      this.eventId = Number(this.route.snapshot.paramMap.get('id'));

      this.eventService.getEvent(this.eventId).subscribe((data: Event) => 
      {
        this.event = data;
      });
  }

  confirmDelete(eventId: number) : void 
  {
    this.eventService.deleteEvent(eventId).subscribe(() => 
    {
      this.router.navigate(['/viewEvents']);
    });
  }

  cancelDelete(): void 
  {
    this.router.navigate(['/viewEvents']);
  }
}

----- event-form.html 

<h2>Add New Event</h2>
<form #eventForm="ngForm" (ngSubmit)="addEvent(eventForm)">
    <div>
        <label>Event Name</label>
        <input type="text" name="eventName" id="eventName" [(ngModel)]="newEvent.eventName" #eventName="ngModel" required/>
        <div class="error-message" *ngIf="formSubmitted && (!newEvent.eventName)">
        Event Name is required</div> 
    </div>

    <div>
        <label>Event Description</label>
        <input type="text" name="eventDescription" id="eventDescription" [(ngModel)]="newEvent.eventDescription" #eventDescription="ngModel" required/>
        <div class="error-message" *ngIf="formSubmitted && (!newEvent.eventDescription)">
        Event Description is required</div> 
    </div>

    <div>
        <label>Event Date</label>
        <input type="text" name="eventDate" id="eventDate" [(ngModel)]="newEvent.eventDate" #eventDate="ngModel" required/>
        <div class="error-message" *ngIf="formSubmitted && (!newEvent.eventDate)">
        Event Date is required</div> 
    </div>

    <div>
        <label>Event Time</label>
        <input type="text" name="eventTime" id="eventTime" [(ngModel)]="newEvent.eventTime" #eventTime="ngModel" required/>
        <div class="error-message" *ngIf="formSubmitted && (!newEvent.eventTime)">
        Event Time is required</div> 
    </div>

    <div>
        <label>Event Location</label>
        <input type="text" name="eventLocation" id="eventLocation" [(ngModel)]="newEvent.eventLocation" #eventLocation="ngModel" required/>
        <div class="error-message" *ngIf="formSubmitted && (!newEvent.eventLocation)">
        Event Location is required</div> 
    </div>
    
    <div>
        <label>Event Organizer</label>
        <input type="text" name="eventOrganizer" id="eventOrganizer" [(ngModel)]="newEvent.eventOrganizer" #eventOrganizer="ngModel" required/>
        <div class="error-message" *ngIf="formSubmitted && (!newEvent.eventOrganizer)">
        Event Organizer is required</div> 
    </div>
    
    <button type="submit">Add Event</button>
</form>  


------ event-form.ts

import { Component } from '@angular/core';
import { Event } from '../models/event.model';
import { EventService } from '../services/event.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent {

  newEvent: Event = {
    eventId: 0,
    eventName: '',
    eventDescription: '',
    eventDate: '',
    eventTime: '',
    eventLocation: '',
    eventOrganizer: ''
  };

  formSubmitted= false;

  constructor(private eventService: EventService, private router: Router){}

  addEvent(eventForm: NgForm): void {
    this.formSubmitted = true;
    if(eventForm.valid)
    {
      this.eventService.addEvent(this.newEvent).subscribe(
        () =>
        {
          this.router.navigate(['/viewEvents']);
        });
    }
  }

}


------ event-form.html 

<h1>Event List</h1>
<div>
<input id="search"  [(ngModel)]="searchTerm"/>
<button class="search-button" (click)="searchEvents()">Search</button>
</div>
<table class="event-table">
    <thead>
        <tr>
            <th>Event Name</th>
            <th>Description</th>
            <th>Date</th>
            <th>Time</th>
            <th>Location</th>
            <th>Organizer</th>
            <th>Action</th>
        </tr>
    </thead>
    <tbody>
        <tr class="event-item" *ngFor="let event of filteredEvents">
            <td>{{event.eventName}}</td>
            <td>{{event.eventDescription}}</td>
            <td>{{event.eventDate}}</td>
            <td>{{event.eventTime}}</td>
            <td>{{event.eventLocation}}</td>
            <td>{{event.eventOrganizer}}</td>
            <td>
                <button class="delete-button" id="delete" (click)="deleteEvent(event.eventId)">Delete</button>
            </td>
        </tr>
    </tbody>
</table>

---- eventlist.ts 

import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { Router } from '@angular/router';
import { Event } from '../models/event.model';
import { RouterTestingHarness } from '@angular/router/testing';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit{
  events: Event[] = [];
  filteredEvents: Event[] = [];
  searchTerm: string ='';

  constructor(private eventService: EventService, private router: Router)
  {}
  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void
  {
    this.eventService.getEvents().subscribe((data: Event[]) => 
    {
      this.events = data;
      this.filteredEvents = data;
    });
  }

  deleteEvent(eventId: number): void 
  {
    this.router.navigate([`/confirmDelete/${eventId}`]);
  }

  searchEvents() : void 
  {
    if(this.searchTerm && this.searchTerm.trim() !== '')
    {
      this.filteredEvents = this.events.filter(e =>
        e.eventName.toLowerCase().includes(this.searchTerm.toLowerCase())
        );
    } else {
      this.filteredEvents = this.events;
    }
  }
}


------header.html 

<h1>Event Management Platform</h1>
<nav>
    <a routerLink="/addNewEvent">Add New Event</a><br>
    <a routerLink="/viewEvents">View Event</a>
</nav>

---- event.model.ts 

export interface Event
{
    eventId: number;
    eventName: string;
    eventDescription: string;
    eventDate: string;
    eventTime: string;
    eventLocation: string;
    eventOrganizer: string;
}

------ eventService 

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Event } from '../models/event.model';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class EventService {

  public apiUrl = 'https://8080-bdfebefbfd352271894beecbcdone.premiumproject.examly.io';
  

  constructor(private httpClient: HttpClient) { }

  addEvent(event: Event): Observable<Event> 
  {
    return this.httpClient.post<Event>(`${this.apiUrl}/api/Event`,event);
  } 

  getEvents(): Observable<Event[]>
  {
    return this.httpClient.get<Event[]>(`${this.apiUrl}/api/Event`);
  }

  deleteEvent(EventId: number): Observable<Event>
  {
    return this.httpClient.delete<Event>(`${this.apiUrl}/api/Event/${EventId}`);
  }

  getEvent(EventId: number): Observable<Event>
  {
    return this.httpClient.get<Event>(`${this.apiUrl}/api/Event/${EventId}`);
  }
}


-------- app-route 

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventFormComponent } from './event-form/event-form.component';
import { EventListComponent } from './event-list/event-list.component';
import { DeleteConfirmComponent } from './delete-confirm/delete-confirm.component';

const routes: Routes = [
  {path: 'addNewEvent', component: EventFormComponent},
  {path: 'viewEvents', component: EventListComponent},
  {path: 'confirmDelete/:id', component: DeleteConfirmComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


---- app.comp.html 

<app-header></app-header>
<router-outlet></router-outlet>

---- app.module.ts 

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { EventFormComponent } from './event-form/event-form.component';
import { EventListComponent } from './event-list/event-list.component';
import { DeleteConfirmComponent } from './delete-confirm/delete-confirm.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    EventFormComponent,
    EventListComponent,
    DeleteConfirmComponent
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

