<div>
    <h2 class="user-name">{{user.name}}</h2>
    <p class="user-age">Age: {{user.age}}</p>
    <p class="user-email">Email: {{user.email}}</p>
 
</div>
 
 
// user-profile html
 
//user-profile component.ts
 
import { Component } from '@angular/core';
import { UserProfile } from 'models/userprofile.model';
 
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {
user: UserProfile={
  name: 'John Doe',
  age: 30,
  email: "john.doe@example.com"
};
}
 
