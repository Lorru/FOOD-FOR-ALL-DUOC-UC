import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SummaryService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findByIdUser(idUser: number): Observable<any>{

    let URL_SUMMARY = this.URL_API + 'Summary/findByIdUser/' + idUser;

    return this.httpClient.get(URL_SUMMARY, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findAllChartDoughnut(): Observable<any>{

    let URL_SUMMARY = this.URL_API + 'Summary/findAllChartDoughnut';

    return this.httpClient.get(URL_SUMMARY, {  headers:  this.headers  });
  }
}
