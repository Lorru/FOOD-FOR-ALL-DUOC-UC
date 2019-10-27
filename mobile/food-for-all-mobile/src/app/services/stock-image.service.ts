import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StockImageService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findByIdStock(idStock: number): Observable<any>{

    let URL_STOCK_IMAGE = this.URL_API + 'StockImage/findByIdStock/' + idStock;

    return this.httpClient.get(URL_STOCK_IMAGE, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(stockImage: any): Observable<any>{

    let URL_STOCK_IMAGE = this.URL_API + 'StockImage/create';

    return this.httpClient.post(URL_STOCK_IMAGE, JSON.stringify(stockImage), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  destroyById(id: number): Observable<any>{

    let URL_STOCK_IMAGE = this.URL_API + 'StockImage/destroyById/' + id;

    return this.httpClient.delete(URL_STOCK_IMAGE, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
