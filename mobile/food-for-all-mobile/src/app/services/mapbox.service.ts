import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapboxService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API_MAPBOX;

  constructor(private httpClient: HttpClient) { }

  getAddressByLongitudeAndLatitude(longitude: number, latitude: number): Observable<any>{

    let URL_MAPBOX = this.URL_API + longitude + ',' + latitude + '.json?access_token=' + environment.MAPBOX_TOKEN;

    return this.httpClient.get(URL_MAPBOX, {  headers:  this.headers});
  }

  
  getAddressBySearch(search: string): Observable<any>{

    let URL_MAPBOX = this.URL_API + search + '.json?access_token=' + environment.MAPBOX_TOKEN;

    return this.httpClient.get(URL_MAPBOX, {  headers:  this.headers});
  }
}
