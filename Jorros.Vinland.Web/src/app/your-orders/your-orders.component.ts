import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { Order } from '../_interfaces/order.model';
import { OrderSummary } from '../_interfaces/order-summary.model';
import { interval, empty } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators'
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
      .pipe(switchMap(() => this.repository.getOrdersByName(this.user.name)
      .pipe(catchError(err => empty()))))
      .subscribe(res => {
        this.orders.data = res.map(x => {
          let ret: OrderSummary =
          {
            id: x.id,
            confirmed: x.bottles.filter(z => z.confirmed).length,
            unconfirmed: x.bottles.filter(z => !z.confirmed).length,
            date: x.date
          }
          return ret;
        });
      }, error => {
        console.log('error appeared')
      });
  }

}
