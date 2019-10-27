import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
;
import { OneSignal } from '@ionic-native/onesignal/ngx';

import { environment } from 'src/environments/environment';
import { NavController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class PushNotificationService {

  private headers = new HttpHeaders().set('Content-Type','application/json');
  private URL_API : string = environment.URL_API_ONESIGNAL;

  playerId: string;

  constructor(private oneSignal: OneSignal, private httpClient: HttpClient, private navController: NavController) { }

  settingsInit(){

    this.oneSignal.startInit(environment.ONESIGNAL_APP_ID, environment.GOOGLE_PROJECT_NUMBER);

    this.oneSignal.inFocusDisplaying(this.oneSignal.OSInFocusDisplayOption.Notification);

    this.oneSignal.handleNotificationReceived().subscribe((res) => {
      
    });

    this.oneSignal.handleNotificationOpened().subscribe((res) => {
      
      this.navController.navigateForward(res.notification.payload.additionalData.url);

    });

    this.oneSignal.endInit();

  }

  getPlayerId(){

    this.oneSignal.getIds().then(res => {

      this.playerId = res.userId;

    })
    
  }

  sendNotification(notification: any): Observable<any>{

    notification.app_id = environment.ONESIGNAL_APP_ID;

    let URL_ONESIGNAL = this.URL_API + 'notifications';

    return this.httpClient.post(URL_ONESIGNAL, JSON.stringify(notification), {  headers:  this.headers.append('Authorization', 'Basic ' + environment.ONESIGNAL_REST_API_KEY)  });
  }
}
