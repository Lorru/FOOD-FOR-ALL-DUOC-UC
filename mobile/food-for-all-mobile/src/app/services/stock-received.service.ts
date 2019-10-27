import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class StockReceivedService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;
  private URL_HUB : string = environment.URL_HUB + 'stockReceived';
  public hubConnection: signalR.HubConnection;

  constructor(private httpClient: HttpClient) { }

  public startConnectionSignalR(){

    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.URL_HUB).build();

    this.hubConnection.start();

  }

  findByIdUserBeneficiary(idUserBeneficiary: number, searcher?: string): Observable<any>{

    let URL_STOCK_RECEIVED = this.URL_API + 'StockReceived/findByIdUserBeneficiary/' + idUserBeneficiary;

    if(searcher){

      URL_STOCK_RECEIVED = URL_STOCK_RECEIVED + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_STOCK_RECEIVED, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findByIdStock(idStock: number): Observable<any>{

    let URL_STOCK_RECEIVED = this.URL_API + 'StockReceived/findByIdStock/' + idStock;

    return this.httpClient.get(URL_STOCK_RECEIVED, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(stockReceived: any): Observable<any>{

    let URL_STOCK_RECEIVED = this.URL_API + 'StockReceived/create';

    return this.httpClient.post(URL_STOCK_RECEIVED, JSON.stringify(stockReceived), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

}
