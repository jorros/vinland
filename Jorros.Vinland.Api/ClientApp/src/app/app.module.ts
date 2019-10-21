import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';

import { RepositoryService } from './shared/services/repository.service'

import { MatCardModule } from '@angular/material/card';
import { OrderComponent } from './order/order.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { LayoutModule } from '@angular/cdk/layout';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { YourOrdersComponent } from './your-orders/your-orders.component'
import { FormsModule } from '@angular/forms';
import { NameLoginComponent } from './name-login/name-login.component';
import { MatDialogModule } from '@angular/material/dialog';
import { UserService } from './shared/services/user.service';

@NgModule({
  declarations: [
    AppComponent,
    OrderComponent,
    YourOrdersComponent,
    NameLoginComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    LayoutModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatDialogModule
  ],
  entryComponents: [NameLoginComponent],
  providers: [
    RepositoryService,
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
