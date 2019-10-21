import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NameLoginComponent } from './name-login/name-login.component';
import { UserService } from './shared/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(public dialog: MatDialog, private user: UserService) { }

  ngOnInit(): void {
    const dialogRef = this.dialog.open(NameLoginComponent, {
      width: '250px',
      data: {},
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      this.user.name = result;
    });
  }
}
