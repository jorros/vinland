import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { Order } from '../_interfaces/order.model';
import { OrderSummary } from '../_interfaces/order-summary.model';
import { interval } from 'rxjs';
import { switchMap } from 'rxjs/operators'
import { RepositoryService } from '../shared/services/repository.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-your-orders',
  templateUrl: './your-orders.component.html',
  styleUrls: ['./your-orders.component.scss']
})
export class YourOrdersComponent implements OnInit {

  constructor(private repository: RepositoryService, private user: UserService) { }

  displayedColumns: string[] = ['id', 'confirmed', 'unconfirmed', 'date'];
  orders = new MatTableDataSource<OrderSummary>();

  ngOnInit() {
    this.refreshOrders();
  }

  public refreshOrders() {
    interval(1000)
      .pipe(switchMap((x) => this.repository.getOrdersByName(this.user.name)))
      .subscribe(res => {
        let result = res as Order[]
        this.orders.data = result.map(x => {
          let ret: OrderSummary =
          {
            id: x.id,
            confirmed: x.bottles.filter(z => z.confirmed).length,
            unconfirmed: x.bottles.filter(z => !z.confirmed).length,
            date: x.date
          }
          return ret;
        });
      });
  }

}
