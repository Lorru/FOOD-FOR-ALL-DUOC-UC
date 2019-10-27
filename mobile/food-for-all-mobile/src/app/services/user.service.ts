import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;
  private URL_HUB : string = environment.URL_HUB + 'user';
  public hubConnection: signalR.HubConnection;

  constructor(private httpClient: HttpClient) { }

  public startConnectionSignalR(){

    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.URL_HUB).build();

    this.hubConnection.start();

  }

  findByIdUserAndFilterDynamic(idUser: number, property: string, condition: string, value: string, searcher?: string): Observable<any>{

    let URL_USER = this.URL_API + 'User/findByIdUserAndFilterDynamic/' + idUser + '/' + property + '/' + condition + '/' + value;

    if(searcher){

      URL_USER = URL_USER + '?searcher=' + searcher;

    }

    return this.httpClient.get(URL_USER, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findById(id: number): Observable<any>{

    let URL_USER = this.URL_API + 'User/findById/' + id;

    return this.httpClient.get(URL_USER, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  findByToken(token: string): Observable<any>{


    let URL_USER = this.URL_API + 'User/findByToken/' + token;

    return this.httpClient.get(URL_USER, {  headers:  this.headers  });
  }

  findByUserNameAndPassword(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/findByUserNameAndPassword';

    return this.httpClient.post(URL_USER, JSON.stringify(user), {  headers:  this.headers  });
  }

  findByEmailWithFacebook(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/findByEmailWithFacebook';

    return this.httpClient.post(URL_USER, JSON.stringify(user), {  headers:  this.headers  });
  }

  create(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/create';

    return this.httpClient.post(URL_USER, JSON.stringify(user), {  headers:  this.headers  });
  }

  sendRecoveryPassword(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/sendRecoveryPassword';

    return this.httpClient.post(URL_USER, JSON.stringify(user), {  headers:  this.headers  });
  }

  updateById(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/updateById';

    return this.httpClient.put(URL_USER, JSON.stringify(user), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updatePasswordById(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/updatePasswordById';

    return this.httpClient.put(URL_USER, JSON.stringify(user), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updateOnLineById(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/updateOnLineById';

    return this.httpClient.put(URL_USER, JSON.stringify(user), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  updatePasswordByIdAndRecovery(user: any): Observable<any>{

    let URL_USER = this.URL_API + 'User/updatePasswordByIdAndRecovery';

    return this.httpClient.put(URL_USER, JSON.stringify(user), {  headers:  this.headers  });
  }

  deleteById(id: number): Observable<any>{

    let URL_USER = this.URL_API + 'User/deleteById/' + id;

    return this.httpClient.delete(URL_USER, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
