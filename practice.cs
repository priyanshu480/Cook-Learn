////////delete-confirm.ts/////////
import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Event } from '../models/event.model';
 
@Component({
  selector: 'app-delete-confirm',
  templateUrl: './delete-confirm.component.html',
  styleUrls: ['./delete-confirm.component.css']
})
export class DeleteConfirmComponent implements OnInit{
  eventId:number;
  event:Event;
  constructor(public objService:EventService,public route:Router,public router:ActivatedRoute){}
  ngOnInit(): void {
      this.router.params.subscribe(p=>{
        this.eventId=+p['id'];
        this.objService.getEvent(this.eventId).subscribe((data)=>this.event=data)
      });
  }
  confirmDelete(eventId:number):void{
    this.objService.deleteEvent(eventId).subscribe(()=>this.route.navigate(['/viewEvents']));
  }
  cancelDelete():void{
    this.route.navigate(['/viewEvents']);
  }
}
 
///////////delete-confirm.html/////////
<h2>Delete Confirmation</h2>
<p>Are you sure you want to delete this event?</p>
<p>Event Name: {{event.eventName}}</p>
<p>Description: {{event.eventDescription}}</p>
<p>Date: {{event.eventDate}}</p>
<p>Time: {{event.eventTime}}</p>
<p>Location: {{event.eventLocation}}</p>
<p>Organizer: {{event.eventOrganizer}}</p>
<button class="confirm-button" type="button" (click)="confirmDelete(eventId)">Confirm Delete</button>
<button class="cancel-button" type="button" (click)="cancelDelete()">Confirm Delete</button>
 
///////event-form.ts///////
 
import { Component } from '@angular/core';
import { EventService } from '../services/event.service';
import { Router } from '@angular/router';
import { Event } from '../models/event.model';
@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent {
  constructor(public objService:EventService,public route:Router){}
  newEvent:Event={
    eventId:0,
    eventName:'',
    eventDescription:'',
    eventDate:'',
    eventTime:'',
    eventLocation:'',
    eventOrganizer:''
  };
  //newEvent:Event;
  formSubmitted:boolean=false;
  addEvent():void{
    this.formSubmitted=true;
    this.objService.addEvent(this.newEvent).subscribe(()=>this.route.navigate(['/viewEvents']));
  }
}
 
//////event-form.html//////
 
<form (ngSubmit)="addEvent()">
<div>
<label for="eventName">Event Name</label>
<input type="text" name="eventName" id="eventName" [(ngModel)]="newEvent.eventName" >
<div class="error-message" *ngIf="!newEvent.eventName">Event Name is required</div>
</div>
<div>
<label for="eventDescription">Event Description</label>
<input type="text" name="eventDescription" id="eventDescription" [(ngModel)]="newEvent.eventDescription" >
<div class="error-message" *ngIf="!newEvent.eventName">Event Description is required</div>
</div>
<div>
<label for="eventDate">Event Date</label>
<input type="date" name="eventDate" id="eventDate" [(ngModel)]="newEvent.eventDate" >
<div class="error-message" *ngIf="!newEvent.eventName">Event Date is required</div>
</div>
<div>
<label for="eventTime">Event Time</label>
<input type="time" name="eventTime" id="eventTime" [(ngModel)]="newEvent.eventTime" >
<div class="error-message" *ngIf="!newEvent.eventTime">Event Time is required</div>
</div>
<div>
<label for="eventLocation">Event Location</label>
<input type="text" name="eventLocation" id="eventLocation" [(ngModel)]="newEvent.eventLocation" >
<div class="error-message" *ngIf="!newEvent.eventLocation">Event Location is required</div>
</div>
<div> 
<label for="eventOrganizer">Event Organizer</label>
<input type="text" name="eventOrganizer" id="eventOrganizer" [(ngModel)]="newEvent.eventOrganizer" >
<div class="error-message" *ngIf="!newEvent.eventOrganizer">Event Organizer is required</div>
</div>
<button type="submit">Add Event</button>
</form>
 
/////////event-list.ts///////
import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { Router } from '@angular/router';
import { Event } from '../models/event.model';
 
@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit{
  events:Event[]=[];
  searchTerm:string;
  filteredEvents:Event[]=[];
  ngOnInit(): void {
      this.loadEvents();
  }
  constructor(public objService:EventService,public route:Router){}
  loadEvents(){
    this.objService.getEvents().subscribe((data)=>{this.events=data;this.filteredEvents=data});
  }
  deleteEvent(eventId:number){
    this.route.navigate(['/confirmDelete',eventId]);
  }
  searchEvents(){
    if(this.searchTerm.trim()==''){
      this.filteredEvents=this.events;
    }
    else{
    this.filteredEvents = this.events.filter(search=>search.eventName.toLowerCase().includes(this.searchTerm.toLowerCase()));
    }
  }
}
///////event-list.html/////
<div>
<input type="text" id="search" name="search" [(ngModel)]="searchTerm">
<button class="search-button" (click)="searchEvents()">Search</button>
<table class="event-table">
<thead>
<tr>
<th>Event Name</th>
<th>Description</th>
<th>Date</th>
<th>Time</th>
<th>Location</th>
<th>Organizer</th>
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
<button class="delete-button" id="delete" (click)="deleteEvent(event.eventId)">Delete</button>
</tr>
</tbody>
</table>
</div>
 
////////header.component.ts///////
 
import { Component } from '@angular/core';
 
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
 
}
 
/////////event.model.ts//////
export interface Event{
    eventId:number;
    eventName:string;
    eventDescription:string;
    eventDate:string;
    eventTime:string;
    eventLocation:string;
    eventOrganizer:string;
}
/////////event.service.ts/////
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Event } from '../models/event.model';
@Injectable({
  providedIn: 'root'
})
export class EventService {
apiUrl:string='https://8080-aadbcafeabacdc352268277beecbcdone.premiumproject.examly.io';
  constructor(private http:HttpClient) { }
  addEvent(Event:Event):Observable<Event>{
   return this.http.post<Event>(this.apiUrl+'/api/Event',Event);
  }
  getEvents():Observable<Event[]>{
    return this.http.get<Event[]>(this.apiUrl+'/api/Event')
  }
  deleteEvent(EventId:number):Observable<void>{
    return this.http.delete<void>(this.apiUrl+'/api/Event/'+EventId);
  }
  getEvent(EventId:number):Observable<Event>{
    return this.http.get<Event>(this.apiUrl+'/api/Event/'+EventId);
  }
}
//////////app-routing.module.ts///////
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventFormComponent } from './event-form/event-form.component';
import { EventListComponent } from './event-list/event-list.component';
import { DeleteConfirmComponent } from './delete-confirm/delete-confirm.component';
 
const routes: Routes = [
  {path:'addNewEvent',component:EventFormComponent},
  {path:'viewEvents',component:EventListComponent},
  {path:'confirmDelete/:id',component:DeleteConfirmComponent}
];
 
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
 
/////////app-module.ts////////
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { EventFormComponent } from './event-form/event-form.component';
import { EventListComponent } from './event-list/event-list.component';
import { DeleteConfirmComponent } from './delete-confirm/delete-confirm.component';
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
 
