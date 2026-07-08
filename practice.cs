task.model.ts
export interface Task {
    id: number;
    title: string;
    description: string;
  }
 
 
app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
 
import { AppComponent } from './app.component';
import { TaskListComponent } from './task-list/task-list.component';
 
@NgModule({
  declarations: [
    AppComponent,
    TaskListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
task-list.html
<div class="container">
 
    <h2>Task List</h2>
 
    <ul
        class="task-item"
        *ngFor="let task of tasks">
 
        <li>{{task.title}} - {{task.description}}</li>
 
    </ul>
 
    <h2>Add Task</h2>
 
    <form (ngSubmit)="addTask()">
 
        <label>Title:</label>
 
        <input
            type="text"
            name="title"
            [(ngModel)]="newTask.title"
            required>
 
        <label>Description:</label>
 
        <textarea
            name="description"
            [(ngModel)]="newTask.description"
            required>
        </textarea>
 
        <button type="submit">
            Add Task
        </button>
 
    </form>
 
</div>
 
task-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Task } from '../model/task.model';
import { TaskService } from '../services/task.service';
 
@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
 
  tasks: Task[] = [];
 
  selectedTask: Task | null = null;
 
  newTask: Task = {
    id: 0,
    title: '',
    description: ''
  };
 
  constructor(private taskService: TaskService) { }
 
  ngOnInit(): void {
    this.loadTasks();
  }
 
  loadTasks(): void {
    this.taskService.getTasks().subscribe(
      (data) => {
        this.tasks = data;
      }
    );
  }
 
  addTask(): void {
 
    const task: Task = {
      id: Date.now(),
      title: this.newTask.title,
      description: this.newTask.description
    };
 
    this.taskService.addTask(task).subscribe(
      () => {
        this.loadTasks();
 
        this.newTask = {
          id: 0,
          title: '',
          description: ''
        };
      }
    );
  }
}
 
task.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../model/task.model';
 
@Injectable({
  providedIn: 'root'
})
export class TaskService {
 
  public apiUrl = 'http://localhost:3000/tasks';
 
  constructor(private http: HttpClient) { }
 
  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl);
  }
 
  addTask(task: Task): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }
}
 
