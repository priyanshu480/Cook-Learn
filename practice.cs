Admin:
 
ts:
----
 
import { Component, OnInit } from '@angular/core';
import { Instructor } from '../../models/instructor.model';
import { Course } from '../../models/course.model';
import { InstructorService } from '../services/instructor.service';
import { CourseService } from '../services/course.service';
 
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
 
  instructors: Instructor[] = [];
  courses: Course[] = [];
 
  editedInstructor: Instructor | null = null;
  editedCourse: Course | null = null;
 
  constructor(
    private instructorService: InstructorService,
    private courseService: CourseService
  ) { }
 
  ngOnInit(): void {
    this.getInstructors();
    this.getCourses();
  }
 
  getInstructors(): void {
    this.instructorService.getInstructors().subscribe(data => {
      this.instructors = data;
    });
  }
 
  editInstructor(instructor: Instructor): void {
    this.editedInstructor = { ...instructor };
  }
 
  saveEditedInstructor(): void {
    if (this.editedInstructor && this.editedInstructor.InstructorId) {
      this.instructorService
        .updateInstructor(this.editedInstructor.InstructorId, this.editedInstructor)
        .subscribe(() => {
          this.getInstructors();
          this.editedInstructor = null;
        });
    }
  }
 
  cancelEditInstructor(): void {
    this.editedInstructor = null;
  }
 
  deleteInstructor(instructorId: number): void {
    this.instructorService.deleteInstructor(instructorId).subscribe(() => {
      this.getInstructors();
    });
  }
 
  getCourses(): void {
    this.courseService.getCourses().subscribe(data => {
      this.courses = data;
    });
  }
 
  editCourse(course: Course): void {
    this.editedCourse = { ...course };
  }
 
  saveEditedCourse(): void {
    if (this.editedCourse && this.editedCourse.CourseId) {
      this.courseService
        .updateCourse(this.editedCourse.CourseId, this.editedCourse)
        .subscribe(() => {
          this.getCourses();
          this.editedCourse = null;
        });
    }
  }
 
  cancelEditCourse(): void {
    this.editedCourse = null;
  }
 
  deleteCourse(courseId: number): void {
    this.courseService.deleteCourse(courseId).subscribe(() => {
      this.getCourses();
    });
  }
}
 
html
-------
 
<div class="admin-container">
  <h1>Admin Panel</h1>
 
  <app-instructor
    [instructors]="instructors"
    [editedInstructor]="editedInstructor"
    (editInstructorEvent)="editInstructor($event)"
    (saveEditedInstructorEvent)="saveEditedInstructor()"
    (cancelEditInstructorEvent)="cancelEditInstructor()"
    (deleteInstructorEvent)="deleteInstructor($event)">
  </app-instructor>
 
  <app-course
    [courses]="courses"
    [editedCourse]="editedCourse"
    (editCourseEvent)="editCourse($event)"
    (saveEditedCourseEvent)="saveEditedCourse()"
    (cancelEditCourseEvent)="cancelEditCourse()"
    (deleteCourseEvent)="deleteCourse($event)">
  </app-course>
</div>
 
course
 
ts
----
 
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Course } from '../../models/course.model';
 
@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent {
 
  @Input() courses: Course[] = [];
  @Input() editedCourse: Course | null = null;
 
  @Output() editCourseEvent = new EventEmitter<Course>();
  @Output() saveEditedCourseEvent = new EventEmitter<void>();
  @Output() cancelEditCourseEvent = new EventEmitter<void>();
  @Output() deleteCourseEvent = new EventEmitter<number>();
 
  onEditCourse(course: Course): void {
    this.editCourseEvent.emit(course);
  }
 
  onSaveEditedCourse(): void {
    this.saveEditedCourseEvent.emit();
  }
 
  onCancelEditCourse(): void {
    this.cancelEditCourseEvent.emit();
  }
 
  onDeleteCourse(courseId: number): void {
    this.deleteCourseEvent.emit(courseId);
  }
 
  getDurationStatus(): string {
    if (!this.editedCourse || !this.editedCourse.Duration || this.editedCourse.Duration === 0) {
      return 'Undefined';
    } else if (this.editedCourse.Duration < 10) {
      return 'Short Duration';
    } else if (this.editedCourse.Duration >= 10 && this.editedCourse.Duration <= 30) {
      return 'Moderate Duration';
    } else {
      return 'Long Duration';
    }
  }
}
 
 
html
-------
<div>
  <h2>Course List</h2>
 
  <table border="1">
    <thead>
      <tr>
        <th>Title</th>
        <th>Description</th>
        <th>Duration</th>
        <th>Actions</th>
      </tr>
    </thead>
 
    <tbody>
      <tr *ngFor="let course of courses">
        <td>{{ course.Title }}</td>
        <td>{{ course.Description }}</td>
        <td>{{ course.Duration }} days</td>
        <td>
          <button (click)="onEditCourse(course)">Edit</button>
          <button (click)="onDeleteCourse(course.CourseId!)">Delete</button>
        </td>
      </tr>
    </tbody>
  </table>
 
  <div *ngIf="editedCourse">
    <h2>Edit Course</h2>
 
    <label>Title:</label>
    <input type="text" [(ngModel)]="editedCourse.Title">
 
    <label>Description:</label>
    <textarea [(ngModel)]="editedCourse.Description"></textarea>
 
    <label>Duration (in days):</label>
    <input type="number" [(ngModel)]="editedCourse.Duration">
 
    <div>
      Duration Status: {{ getDurationStatus() }}
    </div>
 
    <button (click)="onSaveEditedCourse()">Save</button>
    <button (click)="onCancelEditCourse()">Cancel</button>
  </div>
 
</div>
 
homecomponent
 
ts
----
 
import { Component } from '@angular/core';
 
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
 
}
 
 
html:
-------
<div class="home-container">
 
    <h1>Welcome to Course-Instructor Management System</h1>
 
    <p>Please Login or Register to continue.</p>
 
</div>
 
 
 
 
instructor:
 
ts
----
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Instructor } from '../../models/instructor.model';
 
@Component({
  selector: 'app-instructor',
  templateUrl: './instructor.component.html',
  styleUrls: ['./instructor.component.css']
})
export class InstructorComponent {
 
  @Input() instructors: Instructor[] = [];
  @Input() editedInstructor: Instructor | null = null;
 
  @Output() editInstructorEvent = new EventEmitter<Instructor>();
  @Output() saveEditedInstructorEvent = new EventEmitter<void>();
  @Output() cancelEditInstructorEvent = new EventEmitter<void>();
  @Output() deleteInstructorEvent = new EventEmitter<number>();
 
  onEditInstructor(instructor: Instructor): void {
    this.editInstructorEvent.emit(instructor);
  }
 
  onSaveEditedInstructor(): void {
    this.saveEditedInstructorEvent.emit();
  }
 
  onCancelEditInstructor(): void {
    this.cancelEditInstructorEvent.emit();
  }
 
  onDeleteInstructor(instructorId: number): void {
    this.deleteInstructorEvent.emit(instructorId);
  }
 
  onHireDateChange(date: string): void {
    if (this.editedInstructor) {
      this.editedInstructor.HireDate = new Date(date);
    }
  }
}
 
 
html:
-------
 
<div>
  <h2>Instructor List</h2>
 
  <table>
    <thead>
      <tr>
        <th>Instructor Name</th>
        <th>Email</th>
        <th>Hire Date</th>
        <th>Actions</th>
      </tr>
    </thead>
 
    <tbody>
      <tr *ngFor="let instructor of instructors">
        <td>{{ instructor.Name }}</td>
        <td>{{ instructor.Email }}</td>
        <td>{{ instructor.HireDate | date }}</td>
        <td>
          <button (click)="onEditInstructor(instructor)">Edit</button>
          <button (click)="onDeleteInstructor(instructor.InstructorId!)">Delete</button>
        </td>
      </tr>
    </tbody>
  </table>
 
  <div *ngIf="editedInstructor">
    <h2>Edit Instructor</h2>
 
    <label>Instructor Name:</label>
    <input type="text" [(ngModel)]="editedInstructor.Name">
 
    <label>Email:</label>
    <input type="email" [(ngModel)]="editedInstructor.Email">
 
    <label>Hire Date:</label>
    <input
      type="date"
      [ngModel]="editedInstructor.HireDate | date:'yyyy-MM-dd'"
      (ngModelChange)="onHireDateChange($event)">
 
    <button (click)="onSaveEditedInstructor()">Save</button>
    <button (click)="onCancelEditInstructor()">Cancel</button>
  </div>
</div>
 
 
organizer:
 
ts
----
 
import { Component, OnInit } from '@angular/core';
import { Course } from '../../models/course.model';
import { Instructor } from '../../models/instructor.model';
import { CourseService } from '../services/course.service';
import { InstructorService } from '../services/instructor.service';
 
@Component({
  selector: 'app-organizer',
  templateUrl: './organizer.component.html',
  styleUrls: ['./organizer.component.css']
})
export class OrganizerComponent implements OnInit {
 
  courses: Course[] = [];
  unassignedCourses: Course[] = [];
  instructors: Instructor[] = [];
 
  selectedInstructorIds: { [courseId: number]: number } = {};
 
  constructor(
    private courseService: CourseService,
    private instructorService: InstructorService
  ) { }
 
  ngOnInit(): void {
    this.getCourses();
    this.getInstructors();
  }
 
  getCourses(): void {
    this.courseService.getCourses().subscribe(data => {
      this.courses = data;
      this.unassignedCourses = this.courses.filter(course => !course.InstructorId);
    });
  }
 
  getInstructors(): void {
    this.instructorService.getInstructors().subscribe(data => {
      this.instructors = data;
    });
  }
 
  assignCourseToInstructor(course: Course, selectedInstructorId: number): void {
    const updatedCourse: any = {
      ...course,
      InstructorId: selectedInstructorId,
      Instructor: null
    };
 
    this.courseService.updateCourse(course.CourseId!, updatedCourse).subscribe(() => {
      this.getCourses();
      this.getInstructors();
    });
  }
 
  releaseCourseFromInstructor(course: Course, selectedInstructorId: number): void {
    const updatedCourse: any = {
      ...course,
      InstructorId: null,
      Instructor: null
    };
 
    this.courseService.updateCourse(course.CourseId!, updatedCourse).subscribe(() => {
      this.getCourses();
      this.getInstructors();
    });
  }
}
 
 
html
-------
<div class="organizer-container">
 
  <h1>INSTRUCTOR-COURSE ASSIGNMENT PANEL</h1>
 
  <h2>Unassigned Courses</h2>
 
  <p id="no_unassigned" *ngIf="unassignedCourses.length === 0">
    No Unassigned Courses
  </p>
 
  <table class="table table-bordered" *ngIf="unassignedCourses.length > 0">
    <thead>
      <tr>
        <th>Title</th>
        <th>Description</th>
        <th>Duration</th>
        <th>Instructor</th>
        <th>Action</th>
      </tr>
    </thead>
 
    <tbody>
      <tr *ngFor="let course of unassignedCourses">
        <td>{{ course.Title }}</td>
        <td>{{ course.Description }}</td>
        <td>{{ course.Duration }} days</td>
 
        <td>
          <select
            class="form-control"
            name="instructor{{ course.CourseId }}"
            [(ngModel)]="selectedInstructorIds[course.CourseId!]">
 
            <option [ngValue]="undefined">Select Instructor</option>
 
            <option
              *ngFor="let instructor of instructors"
              [ngValue]="instructor.InstructorId">
 
              {{ instructor.Name }} ({{ instructor.Email }})
            </option>
 
          </select>
        </td>
 
        <td>
          <button
            class="btn btn-success"
            (click)="assignCourseToInstructor(course, selectedInstructorIds[course.CourseId!])">
            Assign to Instructor
          </button>
        </td>
 
      </tr>
    </tbody>
  </table>
 
  <h2>Instructor List With Courses</h2>
 
  <p id="no_instructors" *ngIf="instructors.length === 0">
    No Instructors Available
  </p>
 
  <table class="table table-bordered" *ngIf="instructors.length > 0">
    <thead>
      <tr>
        <th>Instructor Name</th>
        <th>Email</th>
        <th>Courses</th>
      </tr>
    </thead>
 
    <tbody>
      <tr *ngFor="let instructor of instructors">
        <td>{{ instructor.Name }}</td>
        <td>{{ instructor.Email }}</td>
 
        <td>
          <table class="table">
            <tbody>
              <tr *ngFor="let course of instructor.Courses">
                <td>{{ course.Title }}</td>
                <td>{{ course.Duration }} days</td>
                <td>
                  <button
                    class="btn btn-success"
                    (click)="releaseCourseFromInstructor(course, instructor.InstructorId!)">
                    Release Course
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </td>
 
      </tr>
    </tbody>
  </table>
 
</div>
 
