<div>
    <h2>Dynamic Message Converter</h2>
    <span>{{messageElement}}</span>
    <button (click)="updateMessage()">Update Message</button>
</div>
 
 
----------------------------
 
import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
 
@Component({
  selector: 'app-dynamic-message',
  templateUrl: './dynamic-message.component.html',
  styleUrls: ['./dynamic-message.component.css']
})
 
export class DynamicMessageComponent {
  messageElement:string = "Initial Message";
 
  updateMessage(){
    this.messageElement="New message generated on button click!"
  }
}
 
 
----------------------------
app.comp.html
 
<app-dynamic-message></app-dynamic-message>
<router-outlet></router-outlet>
 
