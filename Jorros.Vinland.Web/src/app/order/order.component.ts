import { Component, OnInit } from '@angular/core';

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

  submit(): void {
    
  }
}
