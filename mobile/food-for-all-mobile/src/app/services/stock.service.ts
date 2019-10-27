import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;
  private URL_HUB : string = environment.URL_HUB + 'stockAvailable';
  public hubConnection: signalR.HubConnection;

  constructor(private httpClient: HttpClient) { }

  public startConnectionSignalR(){

    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.URL_HUB).build();

    this.hubConnection.start();

  }


  findByIdUser(idUser: number, searcher?: string): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/findByIdUser/' + idUser;

    if(searcher){

      URL_STOCK = URL_STOCK + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findByIdUserAndAvailable(idUser: number, searcher?: string): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/findByIdUserAndAvailable/' + idUser;

    if(searcher){

      URL_STOCK = URL_STOCK + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findAllAvailable(searcher?: string): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/findAllAvailable/';

    if(searcher){

      URL_STOCK = URL_STOCK + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findByIdUserAndFilterDynamic(idUser: number, property: string, condition: string, value: string, searcher?: string): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/findByIdUserAndFilterDynamic/' + idUser + '/' + property + '/' + condition + '/' + value;

    if(searcher){

      URL_STOCK = URL_STOCK + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findById(id: number): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/findById/' + id;

    return this.httpClient.get(URL_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findByIdAndAvailable(id: number): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/findByIdAndAvailable/' + id;

    return this.httpClient.get(URL_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(stock: any): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/create';

    return this.httpClient.post(URL_STOCK, JSON.stringify(stock), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updateById(stock: any): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/updateById';

    return this.httpClient.put(URL_STOCK, JSON.stringify(stock), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updateMassive(stocks: Array<any>): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/updateMassive';

    return this.httpClient.put(URL_STOCK, JSON.stringify(stocks), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updateStatusById(stock: any): Observable<any>{

    let URL_STOCK = this.URL_API + 'Stock/updateStatusById';

    return this.httpClient.put(URL_STOCK, JSON.stringify(stock), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

}
