import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { environment } from './../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient) { }

  public getOrdersByName(name: string) {
    return this.http.get(`${environment.apiUrl}/order/name/${name}`);
  }
}
