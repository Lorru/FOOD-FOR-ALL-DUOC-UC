import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DenouncedService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findByIdUserAndIdUserAccuser(idUser: number, idUserAccuser: number): Observable<any>{

    let URL_DENOUNCED = this.URL_API + 'Denounced/findByIdUserAndIdUserAccuser/' + idUser + '/' + idUserAccuser;

    return this.httpClient.get(URL_DENOUNCED, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(denounced: any): Observable<any>{

    let URL_DENOUNCED = this.URL_API + 'Denounced/create';

    return this.httpClient.post(URL_DENOUNCED, JSON.stringify(denounced), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
