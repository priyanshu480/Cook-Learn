--------- admin.component.html
<div class="container">
<h2>Admin Panel</h2>
<div class="row">
<div class="col-md-6">
<app-team
[teams]="teams"
[editedTeam]="editedTeam"
(editTeamEvent)="editTeam($event)"
(saveEditedTeamEvent)="saveEditedTeam()"
(cancelEditTeamEvent)="cancelEditTeam()"
(deleteTeamEvent)="deleteTeam($event)">
</app-team>
</div>
<div class="col-md-6">
<app-player
[players]="players"
[editedPlayer]="editedPlayer"
(editPlayerEvent)="editPlayer($event)"
(saveEditedPlayerEvent)="saveEditedPlayer()"
(cancelEditPlayerEvent)="cancelEditPlayer()"
(deletePlayerEvent)="deletePlayer($event)">
</app-player>
</div>
</div>
</div>
--------- admin.component.ts
import { Component, OnInit } from '@angular/core';
import { TeamService } from '../services/team.service';
import { PlayerService } from '../services/player.service';
import { Team } from 'src/models/team.model';
import { Player } from 'src/models/player.model';
@Component({
selector: 'app-admin',templateUrl: './admin.component.html',
styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
teams: Team[] = [];
players: Player[] = [];
editedTeam: Team | null = null;
editedPlayer: Player | null = null;
constructor(private teamService: TeamService, private playerService: PlayerService) {}
ngOnInit(): void {
this.getTeams();
this.getPlayers();
}
// --- Team Methods ---
getTeams() {
this.teamService.getTeams().subscribe(data => {
this.teams = data;
});
}
}
editTeam(team: Team) {
this.editedTeam = team;
saveEditedTeam() {
if (this.editedTeam) {
this.teamService.updateTeam(this.editedTeam.Id, this.editedTeam).subscribe(() => {
this.getTeams();
this.editedTeam = null; // Reset form
});
}
}
}
cancelEditTeam() {
this.editedTeam = null;
deleteTeam(teamId: number) {
this.teamService.deleteTeam(teamId).subscribe(() => {
this.getTeams();
});
}
// --- Player Methods ---
getPlayers() {
this.playerService.getPlayers().subscribe(data => {
this.players = data;
});
}
editPlayer(player: Player) {
this.editedPlayer = player;
}
saveEditedPlayer() {if (this.editedPlayer) {
this.playerService.updatePlayer(this.editedPlayer.Id, this.editedPlayer).subscribe(() => {
this.getPlayers();
this.editedPlayer = null;
});
}
}
}
cancelEditPlayer() {
this.editedPlayer = null;
deletePlayer(playerId: number) {
this.playerService.deletePlayer(playerId).subscribe(() => {
this.getPlayers();
});
}
}
--------------- authguard.ts
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
@Injectable({
providedIn: 'root'
})
export class AuthGuard implements CanActivate {
constructor(private authService: AuthService, private router: Router) {}
canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
if (!this.authService.isLoggedin()) {
this.router.navigate(['/login']);
return false;
}
if (this.authService.isAdmin() || this.authService.isOrganizer()) {
return true;
}
// If logged in but lacks proper role
this.router.navigate(['/error']);
return false;
}
}
-------------- home.component.html
<div>
</div>
<h1>Welcome to Premium League Manager</h1>
<p>Please Login or Register to access the dashboard</p>
---------- login.component.html<div class="container">
<h2>Login</h2>
<form #loginForm="ngForm" (ngSubmit)="loginForm.valid && login()">
<label>Username</label>
<input type="text" id="username" name="username"
[(ngModel)]="loginData.Username" required #username (input)="uDirty=true">
<div class="error-message" *ngIf="uDirty && username.value === ''">Username is required</div>
<div>
<label>Password </label>
<input type="password" id="password" name="password"
[(ngModel)]="loginData.Password" required #password (input)="pDirty=true">
<div class="error-message" *ngIf="pDirty && password.value === ''">Password is required</div>
</div>
<button type="submit" id="submit" [disabled]="loginForm.invalid">Login</button>
</form>
</div>
----------- login.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from 'src/models/login-model.model';
import { AuthService } from '../services/auth.service';
@Component({
selector: 'app-login',
templateUrl: './login.component.html',
styleUrls: ['./login.component.css']
})
export class LoginComponent {
loginData: LoginModel = { Username: '', Password: '' };
uDirty =false;
pDirty = false;
constructor(private authService: AuthService, private router: Router) {}
login() {
this.authService.login(this.loginData).subscribe({
next: (res) => {
alert('login Successful!');
if(this.authService.isAdmin()){
this.router.navigate(['/admin']);
} else if (this.authService.isOrganizer())
{
this.router.navigate(['/organiser']);
} else {
this.router.navigate(['/']);
}
},
{
error: (err) =>
alert('Login failed');
}
});}
}
------------- navbar.componet.html
<nav>
<ul>
<li><a routerLink="/">Home</a></li>
<li *ngIf="authService.isOrganizer()"><a routerLink="/organizer">Organizer</a></li>
<li *ngIf="authService.isAdmin()"><a routerLink="/admin">Admin</a></li>
<li *ngIf="authService.isAdmin()"><a routerLink="/admin/createTeam">Create Team</a></li>
<li *ngIf="authService.isAdmin()"><a routerLink="/admin/createPlayer">Create Player</a></li>
<li *ngIf="!authService.isLoggedin()"><a routerLink="/signup">Register</a></li>
<li *ngIf="!authService.isLoggedin()"><a routerLink="/login">Login</a></li>
<li *ngIf="authService.isLoggedin()">
<a href="javascript:void(0)" (click)="logout()">Logout</a>
</li>
</ul>
</nav>
---------- navbar.ts
import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
@Component({
selector: 'app-navbar',
templateUrl: './navbar.component.html',
styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
constructor(public authService: AuthService) {}
logout() {
this.authService.logout();
}
}
----------- organizer.html
<div class="container mt-4">
<h2 class="mb-4">Organizer Dashboard</h2>
<div class="card mb-5">
<div class="card-header bg-primary text-white">
<h4>Unsold Players</h4>
</div>
<div class="card-body">
<div *ngIf="unsoldPlayers.length === 0" id="no_
unsold" class="alert alert-info">
No Unsold Players</div>
<table *ngIf="unsoldPlayers.length > 0" class="table table-striped table-hover mt-3"><thead><tr>
<th>Player Name</th>
<th>Age</th>
<th>Category</th>
<th>Bidding Price</th>
<th>Team</th>
<th>Action</th>
</tr>
</thead>
<tbody>
<tr *ngFor="let player of unsoldPlayers">
<td>{{ player.Name }}</td>
<td>{{ player.Age }}</td>
<td>{{ player.Category }}</td>
<td>{{ player.BiddingPrice | currency:'INR' }}</td>
<td>
<select #teamSelect class="form-select">
<option value="" disabled selected>Select Team</option>
<option *ngFor="let team of teams" [value]="team.Id">{{ team.Name }}</option>
</select>
</td>
<td>
<button class="btn btn-success btn-sm" (click)="assignPlayerToTeam(player, teamSelect.value)">
Assign to Team
</button>
</td>
</tr>
</tbody>
</table>
</div>
</div>
<div class="card">
<div class="card-header bg-dark text-white">
<h4>Teams & Squads</h4>
</div>
<div class="card-body">
<div *ngIf="teams.length === 0" id="no_teams" class="alert alert-warning">
No Teams Available</div>
<div *ngIf="teams.length > 0">
<div *ngFor="let team of teams" class="mb-4 border p-3 rounded bg-light"><h5>{{ team.Name }} <span
class="badge bg-secondary float-end">Remaining Budget: {{ team.MaximumBudget | currency:'INR' }}</span></h5>
<table class="table table-sm mt-3 bg-white">
<thead class="table-dark">
<tr>
<th>Player Name</th>
<th>Age</th>
<th>Category</th>
<th>Action</th>
</tr>
</thead>
<tbody>
<tr *ngFor="let player of players">
<ng-container *ngIf="player.TeamId === team.Id">
<td>{{ player.Name }}</td><td>{{ player.Age }}</td>
<td>{{ player.Category }}</td>
<td>
Release Player
</button>
</td>
</ng-container>
</tr>
</tbody>
</table>
</div>
</div>
<button class="btn btn-danger btn-sm" (click)="releasePlayerFromTeam(player)">
</div>
</div>
</div>
------- organizer.ts
import { Component, OnInit } from'@angular/core';
import { TeamService } from'../services/team.service';
import { PlayerService } from'../services/player.service';
import { Team } from 'src/models/team.model';
import { Player } from 'src/models/player.model';
@Component({
selector: 'app-organizer',
templateUrl: './organizer.component.html',
styleUrls: ['./organizer.component.css']
})
export class OrganizerComponent implements OnInit {
teams:Team[] = [];
players:Player[] = [];
unsoldPlayers:Player[] = [];
constructor(
private teamService:TeamService,
private playerService:PlayerService
) {}
ngOnInit():void {
this.loadData();
}
loadData():void {
this.getTeams();
this.getPlayers();
}getTeams():void {
this.teamService.getTeams().subscribe((data) => {
this.teams=data;
});
}
getPlayers():void {
this.playerService.getPlayers().subscribe((data) => {
this.players=data;
this.unsoldPlayers=this.players.filter(p=>!p.TeamId ||p.TeamId ===0);
});
}
assignPlayerToTeam(player:Player, selectedTeamIdStr:string):void {
const selectedTeamId=Number(selectedTeamIdStr);
if (!selectedTeamId) {
alert("Please select a team.");
return;
}
const team=this.teams.find(t=>t.Id ===selectedTeamId);
if (!team) return;
if (team.MaximumBudget < (player.BiddingPrice ||0)) {
alert("Team does not have enough budget for this player.");
return;
}
player.TeamId =selectedTeamId;
team.MaximumBudget -= (player.BiddingPrice ||0);
this.playerService.updatePlayer(player.Id, player).subscribe(() => {
this.teamService.updateTeam(team.Id, team).subscribe(() => {
alert(`${player.Name} assigned to ${team.Name} successfully!`);
this.loadData(); // Refresh data to update both tables
});
});
}
hasPlayers(teamId: number): boolean {
return this.players.some(p => p.TeamId === teamId);
}
releasePlayerFromTeam(player:Player):void {
// const team =this.teams.find(t=> t.Id === teamId);
// if (!team) return;
// player.TeamId =undefined; // Or 0 depending on your exact backend handling, undefined usually works best for
long?
// team.MaximumBudget += (player.BiddingPrice ||0);
// this.playerService.updatePlayer(player.Id, player).subscribe(() => {
// this.teamService.updateTeam(team.Id, team).subscribe(() => {
// alert(`${player.Name} released from ${team.Name}.`);
// this.loadData();
// });
// });
player.TeamId = null;
if(player.Id)
{this.playerService.updatePlayer(player.Id, player).subscribe(()=> {
this.getPlayers();
this.getTeams();
});
}
}
}
---------- createplayer.html
<div class="container">
<h2>Create Player</h2>
<form #playerForm="ngForm" (ngSubmit)="playerForm.valid && createPlayer()">
<div>
<label>Player Name</label>
<input type="text" id="playerName" name="playerName"
[(ngModel)]="player.Name" required minlength="2" maxlength="50"
#playerName (input)="pDirty=true">
<div class="error-message" *ngIf="pDirty && playerName.value === ''">Player Name is required</div>
</div>
<div>
<label>Player Age</label>
<input type="number" id="playerAge" name="playerAge"
[(ngModel)]="player.Age" required min="18" max="99" #playerAge (input)="aDirty=true">
<div class="error-message" *ngIf="aDirty && playerAge.value === ''">Player Age is required</div>
</div>
<div>
<label>Player Category</label>
<input type="text" id="playerCategory" name="playerCategory"
[(ngModel)]="player.Category" required #playerCategory (input)="cDirty=true">
<div class="error-message" *ngIf="cDirty && playerCategory.value === ''">
Player Category is required
</div>
</div>
<div>
<label>Bidding Price</label>
<input type="number" id="playerBiddingPrice" name="playerBiddingPrice"
[(ngModel)]="player.BiddingPrice" required min="0" #biddingPrice (input)="bDirty=true">
<div class="error-message" *ngIf="bDirty && biddingPrice.value === ''">
Bidding Price is required
</div>
</div>
<div id="status">
Status: {{ biddingPriceStatus }}
</div>
<button type="submit" id="submit" [disabled]="playerForm.invalid">Create Player</button>
</form>
</div>
--------- createplayer.ts
import { Component } from '@angular/core';import { Router } from '@angular/router';
import { Player } from 'src/models/player.model';
@Component({
selector: 'app-create-player',
templateUrl: './create-player.component.html',
styleUrls: ['./create-player.component.css']
})
export class CreatePlayerComponent {
player: Player = {};
pDirty= false;
aDirty = false;
cDirty = false;
bDirty = false;
constructor(private router: Router) {}
get biddingPriceStatus(): string {
const price = this.player.BiddingPrice || 0;
if (price < 1000) return 'Too Low';
if (price < 5000) return 'Low';
return 'Good bidding';
}
createPlayer() {
if (this.player.Age && this.player.Age < 18) {
alert('Player age must be 18 or older.');
} else {
alert('Player created successfully!');
this.router.navigate(['/admin']);
}
}
}
------------ player.comp.html
<div>
<h3>Players List</h3>
<table *ngIf="!editedPlayer" class="table">
<thead>
<tr>
<th>Player Name</th>
<th>Age</th>
<th>Category</th>
<th>Bidding Price</th>
<th>Actions</th>
</tr>
</thead>
<tbody>
<tr *ngFor="let player of players">
<td>{{ player.Name }}</td>
<td>{{ player.Age }}</td>
<td>{{ player.Category }}</td>
<td>{{ player.BiddingPrice }}</td>
<td>
<button (click)="onEditPlayer(player)">Edit</button><button (click)="onDeletePlayer(player.Id)">Delete</button>
</td>
</tr>
</tbody>
</table>
<div *ngIf="editedPlayer">
<h4>Edit Player</h4>
<form (ngSubmit)="onSaveEditedPlayer()">
<div>
<label>Player Name</label>
<input type="text" name="name" [(ngModel)]="editedPlayer.Name" required>
</div>
<div>
<label>Age</label>
<input type="number" name="age" [(ngModel)]="editedPlayer.Age" required>
</div>
<div>
<label>Category</label>
<input type="text" name="category" [(ngModel)]="editedPlayer.Category" required>
</div>
<div>
<label>Bidding Price</label>
<input type="number" name="price" [(ngModel)]="editedPlayer.BiddingPrice" required>
</div>
<button type="submit">Save</button>
<button type="button" (click)="onCancelEditPlayer()">Cancel</button>
</form>
</div>
</div>
------- player.comp.ts
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Player } from 'src/models/player.model';
@Component({
selector: 'app-player',
templateUrl: './player.component.html',
styleUrls: ['./player.component.css']
})
export class PlayerComponent {
@Input() players: Player[] = [];
@Input() editedPlayer: Player | null = null;
@Output() editPlayerEvent = new EventEmitter<Player>();
@Output() saveEditedPlayerEvent = new EventEmitter<Player>();
@Output() cancelEditPlayerEvent = new EventEmitter<void>();
@Output() deletePlayerEvent = new EventEmitter<number>();
onEditPlayer(player: Player) {
this.editPlayerEvent.emit({ ...player });
}
onSaveEditedPlayer() {
if (this.editedPlayer) {
this.saveEditedPlayerEvent.emit(this.editedPlayer);
}
}onCancelEditPlayer() {
this.cancelEditPlayerEvent.emit();
}
onDeletePlayer(playerId: number) {
if (confirm('Are you sure you want to delete this player?')) {
this.deletePlayerEvent.emit(playerId);
}
}
}
----------- registration.html
<div class="container">
<h2>Register</h2>
<form #registerForm="ngForm" (ngSubmit)="registerForm.valid && register()">
<div>
<label>Username</label>
<input type="text" id="username" name="username"
[(ngModel)]="user.Username" required #username (input)="uDirty=true">
<div class="error-message" *ngIf="uDirty && username.value === ''">
Username is required
</div>
</div>
<div>
<label>Password</label>
<input type="password" id="password" name="password"
[(ngModel)]="user.Password" required
pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{4,}$"
#password (input)="pDirty=true">
<div class="error-message" *ngIf="pDirty && password.value === ''">
Password is required
</div>
</div>
<div>
<label>Confirm Password</label>
<input type="password" id="confirmPassword" name="confirmPassword"
[(ngModel)]="confirmPassword" required #confirmPwd (input)="cDirty=true">
<div class="error-message" *ngIf="cDirty && confirmPwd.value === ''"> Confirm Password is required</div>
<div class="error-message" *ngIf="cDirty && confirmPwd.value !== '' && confirmPwd.value !==
password.value">Passwords do not match</div>
</div>
<div>
<label>Role</label>
<select id="role" name="role" [(ngModel)]="user.Role" required #role (change)="rDirty=true">
<option value="Admin">Admin</option>
<option value="Organizer">Organizer</option>
</select>
<div class="error-message" *ngIf="rDirty && role.value === ''">
Role is required.
</div>
</div>
<button type="submit" id="submit"
[disabled]="registerForm.invalid || confirmPassword !== user.Password">Register</button></form>
</div>
---------- reg.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/models/user.model';
import { AuthService } from '../services/auth.service';
@Component({
selector: 'app-registration',
templateUrl: './registration.component.html',
styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
user: User = { Username: '', Password: '', Role: 'Organizer' };
confirmPassword = '';
uDirty = false;
pDirty = false;
cDirty = false;
rDirty = false;
constructor(private authService: AuthService, private router: Router) {}
register() {
this.authService.register(this.user).subscribe({
next: (res) => {
alert('Registration successfull !');
this.router.navigate(['/login']);
},
error: (err) => {
alert('Registration failed');
}
});
}
}
---------- authservice
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { User } from 'src/models/user.model';
import { LoginModel } from 'src/models/login-model.model';
import { Router } from '@angular/router';
@Injectable({
providedIn: 'root'
})
export class AuthService {
public baseUrl = 'https://ide-bdfebefbfd351884805dceaabaeafdfone.premiumproject.examly.io/proxy/8080/api/users';
private isAuthenticatedSubject = new BehaviorSubject<any>(this.isLoggedin());
private storeUserDate(userData: any) : void{
if(!userData) return;
const role = userData.role || userData.Role;
if(role)
{
localStorage.setItem('role', role);
}
localStorage.setItem('token','auth-token');
}
constructor(private http: HttpClient)
{}
register(newUser: User) : Observable<User>{
return this.http.post<User>(`${this.baseUrl}/register`, newUser).pipe
(tap(res => this.storeUserDate(res))
);
}
{
login(loginUser: LoginModel) : Observable<any>
return this.http.post<any>(`${this.baseUrl}/login`, loginUser).pipe(
tap(res => {
const userData = res && res.user ? res.user : res;
this.storeUserDate(userData);
this.updatedAuthenticationStatus(false);
})
);
}
{
logout() : void
localStorage.clear();
this.updatedAuthenticationStatus(false);
}
}
}
}
{
}
isLoggedin() : boolean{
return !!localStorage.getItem('role');
isAdmin(): boolean {
return localStorage.getItem('role') === 'Admin';
isOrganizer(): boolean {
return localStorage.getItem('role') === 'Organizer';
updatedAuthenticationStatus(isAuthenticated : boolean) : void
this.isAuthenticatedSubject.next(isAuthenticated);
}
---------------- playerservice
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';import { Player } from 'src/models/player.model';
@Injectable({
providedIn: 'root'
})
export class PlayerService {
public baseUrl = 'http://ide-bdfebefbfd351884805dceaabaeafdfone.premiumproject.examly.io/proxy/8080/Player';
constructor(private http: HttpClient) { }
getPlayers(): Observable<Player[]> {
return this.http.get<Player[]>(`${this.baseUrl}/GetPlayers`);
}
}
}
}
createPlayer(player: Player): Observable<Player> {
return this.http.post<Player>(`${this.baseUrl}/PostPlayer`, player);
updatePlayer(playerId: number, player: Player): Observable<Player> {
return this.http.put<Player>(`${this.baseUrl}/PutPlayer/${playerId}`, player);
deletePlayer(playerId: number): Observable<void> {
return this.http.delete <void>(`${this.baseUrl}/DeletePlayer/${playerId}`);
}
------------- teamservice
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Team } from 'src/models/team.model';
@Injectable({
providedIn: 'root'
})
export class TeamService {
public baseUrl = 'http://ide-bdfebefbfd351884805dceaabaeafdfone.premiumproject.examly.io/proxy/8080/Team';
constructor(private http: HttpClient) { }
getTeams(): Observable<Team[]> {
return this.http.get<Team[]>(`${this.baseUrl}/GetTeams`);
}
}
}
createTeam(team: Team): Observable<Team> {
return this.http.post<Team>(`${this.baseUrl}/PostTeam`, team);
updateTeam(teamId: number, team: Team): Observable<Team> {
return this.http.put<Team>(`${this.baseUrl}/PutTeam/${teamId}`, team);
deleteTeam(teamId: number): Observable <void> {
return this.http.delete <void>(`${this.baseUrl}/DeleteTeam/${teamId}`);}
}
----------- createteam.html
<div class="container">
<h2>Create Team</h2>
<form #teamForm="ngForm" (ngSubmit)="teamForm.valid && createTeam()">
<div>
<label>Team Name</label>
<input type="text" id="teamName" name="teamName" [(ngModel)]="team.Name" required minlength="2"
maxlength="50" #teamName="ngModel">
<div class="error-message" *ngIf="teamName.invalid && (teamName.dirty || teamName.touched)">
<div *ngIf="teamName.errors?.['required']">Team Name is required</div>
</div>
</div>
<div>
<label>Maximum Budget</label>
<input type="number" id="maximumBudget" name="maximumBudget" [(ngModel)]="team.MaximumBudget"
required #maxBudget="ngModel">
<div class="error-message" *ngIf="maxBudget.invalid && (maxBudget.dirty || maxBudget.touched)">
<div *ngIf="maxBudget.errors?.['required']">Maximum Budget is required</div>
</div>
</div>
<div id="status">
Budget Status: {{ maxBidStatus }}
</div>
<button type="submit" id="submit" [disabled]="teamForm.invalid">Create Team</button>
</form>
</div>
---------- createteam.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Team } from 'src/models/team.model';
@Component({
selector: 'app-create-team',
templateUrl: './create-team.component.html',
styleUrls: ['./create-team.component.css']
})
export class CreateTeamComponent {
team: Team = { Id: 0, Name: '', MaximumBudget: 0 };
constructor(private router: Router) {}
get maxBidStatus(): string {
const budget = this.team.MaximumBudget;
if (budget === null || budget <= 0) return 'Bad Budget';
if (budget < 1000) return 'Too Low';
if (budget < 5000) return 'Low';
return 'Good Budget';
}createTeam() {
// Note: AdminService call will be integrated later.
alert('Team created successfully!');
this.router.navigate(['/admin']);
}
}
---------- team.comp.html
<div>
<h3>Teams List</h3>
<table *ngIf="!editedTeam" class="table">
<thead>
<tr>
<th>Team Name</th>
<th>Budget</th>
<th>Actions</th>
</tr>
</thead>
<tbody>
<tr *ngFor="let team of teams">
<td>{{ team.Name }}</td>
<td>{{ team.MaximumBudget }}</td>
<td>
<button (click)="onEditTeam(team)">Edit</button>
<button (click)="onDeleteTeam(team.Id)">Delete</button>
</td>
</tr>
</tbody>
</table>
<div *ngIf="editedTeam">
<h4>Edit Team</h4>
<form (ngSubmit)="onSaveEditedTeam()">
<div>
<label>Team Name</label>
<input type="text" name="name" [(ngModel)]="editedTeam.Name" required>
</div>
<div>
<label>Budget</label>
<input type="number" name="budget" [(ngModel)]="editedTeam.MaximumBudget" required>
</div>
<button type="submit">Save</button>
<button type="button" (click)="onCancelEditTeam()">Cancel</button>
</form>
</div>
</div>
----------- team.comp.ts
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Team } from 'src/models/team.model';
@Component({
selector: 'app-team',templateUrl: './team.component.html',
styleUrls: ['./team.component.css']
})
export class TeamComponent {
@Input() teams: Team[] = [];
@Input() editedTeam: Team | null = null;
@Output() editTeamEvent = new EventEmitter<Team>();
@Output() saveEditedTeamEvent = new EventEmitter<Team>();
@Output() cancelEditTeamEvent = new EventEmitter<void>();
@Output() deleteTeamEvent = new EventEmitter<number>();
onEditTeam(team: Team) {
this.editTeamEvent.emit({ ...team });
}
onSaveEditedTeam() {
if (this.editedTeam) {
this.saveEditedTeamEvent.emit(this.editedTeam);
}
}
}
onCancelEditTeam() {
this.cancelEditTeamEvent.emit();
onDeleteTeam(teamId: number) {
if (confirm('Are you sure you want to delete this team?')) {
this.deleteTeamEvent.emit(teamId);
}
}
}
--------app-routing
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { CreateTeamComponent } from './team/create-team/create-team.component';
import { CreatePlayerComponent } from './player/create-player/create-player.component';
import { OrganizerComponent } from './organizer/organizer.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { ErrorComponent } from './error/error.component';
import { AuthGuard } from './authguard/auth.guard';
const routes: Routes = [
{ path: '', component: HomeComponent },
{ path: 'admin', component: AdminComponent,canActivate: [AuthGuard] },
{ path: 'admin/createTeam', component: CreateTeamComponent, canActivate: [AuthGuard] },
{ path: 'admin/createPlayer', component: CreatePlayerComponent, canActivate: [AuthGuard] },
{ path: 'organizer', component: OrganizerComponent, canActivate: [AuthGuard] },
{ path: 'login', component: LoginComponent },
{ path: 'signup', component: RegistrationComponent },
{ path: 'error', component: ErrorComponent },
{ path: '**', redirectTo: '/error' }
];@NgModule({
imports: [RouterModule.forRoot(routes)],
exports: [RouterModule]
})
export class AppRoutingModule { }
--------APP.COMP.ht
<app-navbar></app-navbar>
<div class="container mt-4">
<router-outlet></router-outlet>
</div>
------ app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { AdminComponent } from './admin/admin.component';
import { OrganizerComponent } from './organizer/organizer.component';
import { PlayerComponent } from './player/player.component';
import { TeamComponent } from './team/team.component';
import { HomeComponent } from './home/home.component';
import { ErrorComponent } from './error/error.component';
import { FormsModule } from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import { CreatePlayerComponent } from './player/create-player/create-player.component';
import { CreateTeamComponent } from './team/create-team/create-team.component';
@NgModule({
declarations: [
AppComponent,
RegistrationComponent,
LoginComponent,
NavbarComponent,
AdminComponent,
OrganizerComponent,
PlayerComponent,
TeamComponent,
HomeComponent,
ErrorComponent,
CreatePlayerComponent,
CreateTeamComponent
],
imports: [
BrowserModule,
AppRoutingModule,
FormsModule,
HttpClientModule
],
providers: [],
bootstrap: [AppComponent]})
export class AppModule { }
--------- models (outside app)
---- login model
export interface LoginModel
{
Username? : string;
Password? : string;
}
------ playermodel
import { Team } from "./team.model";
export interface Player
{
Id? : any;
Name? : string;
Age? : number;
Category? : string;
BiddingPrice? : number;
TeamId? : number;
Team? : Team;
}
----- teammodel
export interface Team
{
Id?: number;
Name: string;
MaximumBudget: number;
}
------- user model
export interface User{
Id? : number;
Username? : string;
Password? : string;
Role? : "Admin" | "Organizer";
}
---------- Program.cs
using System.Text.Json;
using System.Text.Json.Serialization;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.builder.Services.AddControllers()
.AddJsonOptions(options =>
{
options.JsonSerializerOptions.PropertyNamingPolicy = null;
options.JsonSerializerOptions.DictionaryKeyPolicy = null;
options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}
);
builder.Services.AddDbContext<ApplicationDbContext>(options
=> options.UseSqlServer("User ID=sa;password=examlyMssql@123;server=localhost;database=
appdb;trusted_
connection=false;encrypt=false;Persist Security Info=false;"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
options.AddPolicy("AllowAll", policy =>
{
policy.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader();
});
});
builder.WebHost.UseUrls("http://0.0.0.0:8080");
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
