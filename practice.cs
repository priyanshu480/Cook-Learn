import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
 
  name: 'searchFilter'
 
})
 
export class SearchFilterPipe implements PipeTransform {
 
  transform(items:any[] ,searchterm:string): any[] {
    if(!items || !searchterm){
    return items;
 
  }
 
    searchterm = searchterm.toLowerCase();
    return items.filter(item=> JSON.stringify(item).toLowerCase().includes(searchterm));
 
  }
 
}
 
import { Component } from '@angular/core';
 
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
 
  searchText:string='';
 
items = [
 
  {id:1,name:'Apple',category:'Fruit'},
 
  {id:2,name:'Banana',category:'Fruit'},
 
  {id:3,name:'Carrot',category:'Vegetable'}
 
   ];
}
 
 
 
 
 
<input type="text" placeholder="Search..." [(ngModel)]="searchText">
 
<div>
 
    <ul>
 
    <li *ngFor="let item of items | searchFilter: searchText">
 
    {{item.name}} - {{item.category}}
 
    </li>
 
    </ul>
 
</div>
 
<app-search></app-search>
<router-outlet></router-outlet>
 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchComponent } from './search/search.component';
import { SearchFilterPipe } from './pipes/search-filter.pipe';
import { FormsModule } from '@angular/forms';
 
@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    SearchFilterPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 
 
import { SearchFilterPipe } from './search-filter.pipe';
describe('SearchFilterPipe', () => {
  let pipe: SearchFilterPipe;
 
  beforeEach(() => {
    pipe = new SearchFilterPipe();
  });
  fit('create_an_instance', () => {
    const pipe = new SearchFilterPipe();
    expect(pipe).toBeTruthy();
  });
 
  fit('should_return_the_original_items_if_either_items_or_searchTerm_is_falsy', () => {
    const items = [
      { id: 1, name: 'Apple', category: 'Fruit' },
      { id: 2, name: 'Banana', category: 'Fruit' },
      { id: 3, name: 'Carrot', category: 'Vegetable' }
    ];
    const searchTerm = '';
    const result = (pipe as any).transform(items, searchTerm);
    expect(result).toEqual(items);
  });
 
  fit('should_return_filtered_items_based_on_the_search_term', () => {
    const items = [
      { id: 1, name: 'Apple', category: 'Fruit' },
      { id: 2, name: 'Banana', category: 'Fruit' },
      { id: 3, name: 'Carrot', category: 'Vegetable' }
    ];
    const searchTerm = 'apple';
    const result = (pipe as any).transform(items, searchTerm);
    expect(result).toEqual([{ id: 1, name: 'Apple', category: 'Fruit' }]);
  });
 
});
 
 
