import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API;
  private URL_HUB : string = environment.URL_HUB + 'message';
  public hubConnection: signalR.HubConnection;

  constructor(private httpClient: HttpClient) { }

  public startConnectionSignalR(){

    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.URL_HUB).build();

    this.hubConnection.start();

  }

  findByIdUserLast(idUser: number): Observable<any>{

    let URL_MESSAGE = this.URL_API + 'Message/findByIdUserLast/' + idUser;

    return this.httpClient.get(URL_MESSAGE, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }


  findByIdUserSendAndIdUserReceived(idUserSend: number, idUserReceived: number): Observable<any>{

    let URL_MESSAGE = this.URL_API + 'Message/findByIdUserSendAndIdUserReceived/' + idUserSend + '/' + idUserReceived;

    return this.httpClient.get(URL_MESSAGE, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  create(message: any): Observable<any>{

    let URL_MESSAGE = this.URL_API + 'Message/create';

    return this.httpClient.post(URL_MESSAGE, JSON.stringify(message), {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }

  destroyById(id: number): Observable<any>{

    let URL_MESSAGE = this.URL_API + 'Message/destroyById/' + id;

    return this.httpClient.delete(URL_MESSAGE, {  headers:  this.headers.append('Authorization', localStorage.getItem('token'))  });
  }
}
