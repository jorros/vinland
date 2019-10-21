import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoginData } from '../_interfaces/login-data.model';

@Component({
  selector: 'app-name-login',
  templateUrl: './name-login.component.html',
  styleUrls: ['./name-login.component.scss']
})
export class NameLoginComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<NameLoginComponent>,
    @Inject(MAT_DIALOG_DATA) public data: LoginData) { }

  ngOnInit() {
  }

}
