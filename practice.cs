export class Product{
    id: number;
    name: string;
    category: string;
    price: number;
}
 
<h1>Product List</h1>
 
<table>
  <thead>
    <tr>
      <th>ID</th>
      <th>Name</th>
      <th>Category</th>
      <th>Price</th>
    </tr>
  </thead>
  <tbody>
    <tr *ngFor = "let item of productData">
      <td>{{item.id}}</td>
      <td>{{item.name}}</td>
      <td>{{item.category}}</td>
      <td>{{item.price}}</td>
    </tr>
  </tbody>
</table>
 
import { Component } from '@angular/core';
import { Product } from './product.model';
 
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  //title = 'angularapp';
  productData : Product[] = [{
    id: 1,
    name: "Headphone",
    category: "Electronic",
    price: 100000
  },
  {
    id: 2,
    name: "Bottle",
    category: "Bottle",
    price: 8000
 
  }
]
}
 
 
