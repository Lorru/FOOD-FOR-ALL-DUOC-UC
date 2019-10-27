import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalificationStockService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findByIdStockAndIdUserCalification(idStock: number, idUserCalification: number): Observable<any>{

    let URL_CALIFICATION_STOCK = this.URL_API + 'CalificationStock/findByIdStockAndIdUserCalification/' + idStock + '/' + idUserCalification;

    return this.httpClient.get(URL_CALIFICATION_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(calificationStock: any): Observable<any>{

    let URL_CALIFICATION_STOCK = this.URL_API + 'CalificationStock/create';

    return this.httpClient.post(URL_CALIFICATION_STOCK, JSON.stringify(calificationStock), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  destroyById(id: number): Observable<any>{

    let URL_CALIFICATION_STOCK = this.URL_API + 'CalificationStock/destroyById/' + id;

    return this.httpClient.delete(URL_CALIFICATION_STOCK, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
