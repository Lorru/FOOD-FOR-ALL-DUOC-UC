import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class StockAvailableService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }


  create(stockAvailable: any): Observable<any>{

    let URL_STOCK_AVAILABLE = this.URL_API + 'StockAvailable/create';

    return this.httpClient.post(URL_STOCK_AVAILABLE, JSON.stringify(stockAvailable), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }


  destroyByIdStock(idStock: number): Observable<any>{

    let URL_STOCK_AVAILABLE = this.URL_API + 'StockAvailable/destroyByIdStock/' + idStock;

    return this.httpClient.delete(URL_STOCK_AVAILABLE, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
