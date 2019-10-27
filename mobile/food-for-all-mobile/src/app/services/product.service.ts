import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findAll(searcher? : string): Observable<any>{

    let URL_PRODUCT = this.URL_API + 'Product/findAll';

    if(searcher){

      URL_PRODUCT = URL_PRODUCT + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_PRODUCT, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
