import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { environment } from './../../../environments/environment';
import { Order } from 'src/app/_interfaces/order.model';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient) { }

  public getOrdersByName(name: string) {
    return this.http.get<Order[]>(`${environment.apiUrl}/order/name/${name}`);
  }

  public createNewOrder(user: string, amount: number) {
    return this.http.post(
            `${environment.apiUrl}/order`, 
            { user, bottlesAmount: amount }, 
            { 
              headers: new HttpHeaders({ 'Content-Type': 'application/json' })
            }
          )
  }
}
