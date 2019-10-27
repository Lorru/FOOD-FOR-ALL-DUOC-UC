import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;

  constructor(private httpClient: HttpClient) { }

  findByIdUser(idUser: number): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/findByIdUser/' + idUser;

    return this.httpClient.get(URL_LOCATION, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findAllStockAvailable(searcher?: string, isSearchLocation?: boolean): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/findAllStockAvailable/';

    if(searcher){

      URL_LOCATION = URL_LOCATION + '?searcher=' + searcher;

    }

    if(isSearchLocation){

      URL_LOCATION = URL_LOCATION + '?isSearchLocation=' + isSearchLocation;

    }

    return this.httpClient.get(URL_LOCATION, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findById(id: number): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/findById/' + id;

    return this.httpClient.get(URL_LOCATION, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findByIdUserAndMain(idUser: number): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/findByIdUserAndMain/' + idUser;

    return this.httpClient.get(URL_LOCATION, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(location: any): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/create';

    return this.httpClient.post(URL_LOCATION, JSON.stringify(location), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updateIsMainById(location: any): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/updateIsMainById';

    return this.httpClient.put(URL_LOCATION, JSON.stringify(location), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  deleteById(id: number): Observable<any>{

    let URL_LOCATION = this.URL_API + 'Location/deleteById/' + id;

    return this.httpClient.delete(URL_LOCATION, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
