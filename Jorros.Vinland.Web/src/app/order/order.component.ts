import { Component } from '@angular/core';

import { RepositoryService } from '../shared/services/repository.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent {
  constructor(private repository: RepositoryService, private user: UserService) { }

  amount: number;

  onSubmit() {
    this.repository.createNewOrder(this.user.name, this.amount)
      .subscribe(x => {
        this.amount = 0
      }, err => {
        console.log(err)
      })
  }
}
