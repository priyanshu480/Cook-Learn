s1w6d3

q1

============

book-list.component.ts

import { Component } from '@angular/core';
 
interface Book{

  title : string;

  completed: boolean;

}
 
@Component({

  selector: 'app-book-list',

  templateUrl: './book-list.component.html',

  styleUrls: ['./book-list.component.css']

})

export class BookListComponent {

  books : Book[]=[];

  newBookTitle: string='';

  addBook():void

  {

   let trimmedNetBook :string=this.newBookTitle.trim();

    if(trimmedNetBook){

      this.books.push({title:trimmedNetBook,completed:false});

      this.newBookTitle='';

    }

  }

  completeBook(book:Book)

  {

    book.completed = !book.completed;

  }
 
  deleteBook(index:number)

  {

    this.books.splice(index,1);

  }
 
}

==========

book-list.component.html
<div>
<input type="text" [(ngModel)]="newBookTitle" placeholder="Enter book title"/>
<button (click)="addBook()">Add Book</button>
</div>
<ul>
<li *ngFor="let item of books; let i=index ">
<span [style.text-decoration]="item.completed?'line-through':'none'">{{item.title}}</span>
<button (click)="completeBook(item)">Completed</button>
<button (click)="deleteBook(i)">Delete</button>
</li>
</ul>

=======

app.component.html
<app-book-list></app-book-list>

=================================

q2

shopping-list.component.ts

import { Component } from '@angular/core';
 
interface Item {
 
  name: string;
 
  purchased: boolean;
 
}
 
@Component({
 
  selector: 'app-shopping-list',
 
  templateUrl: './shopping-list.component.html',
 
  styleUrls: ['./shopping-list.component.css']
 
})
 
export class ShoppingListComponent {
 
  items: Item[] = [];
 
  newItemName: string = '';
 
  addItem(): void {
 
    if (this.newItemName.trim() !== '') {
 
    this.items.push({
 
    name: this.newItemName,
 
    purchased: false
 
    });
 
    this.newItemName = '';
 
    }
 
  }
 
  purchaseItem(item: Item): void {
 
    item.purchased = !item.purchased;
 
  }
 
  deleteItem(index: number): void {
 
    this.items.splice(index, 1);
 
  }
 
}

===========

shopping-list.component.html
<h2>Shopping List</h2>
 
<input
 
  type="text"
 
  [(ngModel)]="newItemName"
 
  placeholder="Enter item name">
 
<button (click)="addItem()">Add Item`</button>`
 
<ul>
 
<li *ngFor="let item of items; let i = index">
 
    <span>{{ item.name }}</span>
 
    <button (click)="purchaseItem(item)">
 
    Purchased
 
    </button>
 
    <button (click)="deleteItem(i)">
 
    Delete
 
    </button>
 
</li>
 
</ul>
 
==========

app.component.html
<app-shopping-list></app-shopping-list>
 
