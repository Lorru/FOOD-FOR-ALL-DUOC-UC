import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InstitutionService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findAll(searcher? : string): Observable<any>{

    let URL_INSTITUTION = this.URL_API + 'Institution/findAll';

    if(searcher){

      URL_INSTITUTION = URL_INSTITUTION + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_INSTITUTION, {  headers:  this.headers  });
  }
}
