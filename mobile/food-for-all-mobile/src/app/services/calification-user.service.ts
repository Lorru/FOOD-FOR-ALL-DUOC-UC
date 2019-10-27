import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalificationUserService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findByIdUserAndIdUserCalification(idUser: number, idUserCalification: number): Observable<any>{

    let URL_CALIFICATION_USER = this.URL_API + 'CalificationUser/findByIdUserAndIdUserCalification/' + idUser + '/' + idUserCalification;

    return this.httpClient.get(URL_CALIFICATION_USER, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(calificationUser: any): Observable<any>{

    let URL_CALIFICATION_USER = this.URL_API + 'CalificationUser/create';

    return this.httpClient.post(URL_CALIFICATION_USER, JSON.stringify(calificationUser), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  destroyById(id: number): Observable<any>{

    let URL_CALIFICATION_USER = this.URL_API + 'CalificationUser/destroyById/' + id;

    return this.httpClient.delete(URL_CALIFICATION_USER, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
