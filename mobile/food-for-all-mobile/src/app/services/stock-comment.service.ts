import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class StockCommentService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;
  private URL_HUB : string = environment.URL_HUB + 'stockComment';
  public hubConnection: signalR.HubConnection;

  constructor(private httpClient: HttpClient) { }

  public startConnectionSignalR(){

    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.URL_HUB).build();

    this.hubConnection.start();

  }

  findByIdStock(idStock: number): Observable<any>{

    let URL_STOCK_COMMENT = this.URL_API + 'StockComment/findByIdStock/' + idStock;

    return this.httpClient.get(URL_STOCK_COMMENT, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(stockComment: any): Observable<any>{

    let URL_STOCK_COMMENT = this.URL_API + 'StockComment/create';

    return this.httpClient.post(URL_STOCK_COMMENT, JSON.stringify(stockComment), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  destroyById(id: number): Observable<any>{

    let URL_STOCK_COMMENT = this.URL_API + 'StockComment/destroyById/' + id;

    return this.httpClient.delete(URL_STOCK_COMMENT, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

}
