import {  inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { OrderToCreate,Order } from '../../shared/models/order';


@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  orderComplate = false;
  createOrder(orderTocreate: OrderToCreate) {
    return this.http.post<Order>(this.baseUrl + 'orders', orderTocreate);
  }

  getOrdersForUser() {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }
  
  getOrderById(id: number) {
    return this.http.get<Order>(this.baseUrl + 'orders/' + id);
  }
  
}
