import { Component } from '@angular/core';

import { Platform } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';

import { PushNotificationService } from './services/push-notification.service';
import { UserService } from './services/user.service';
import { ToastService } from './services/toast.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
  providers:[
    PushNotificationService,
    UserService,
    ToastService
  ]
})
export class AppComponent {

  user: any;

  constructor(private platform: Platform, private splashScreen: SplashScreen, private statusBar: StatusBar, private _pushNotificationService: PushNotificationService, private _userService: UserService, private _toastService: ToastService) {

    this.initializeApp();

  }

  initializeApp() {

    this.platform.ready().then(() => {

      this.statusBar.styleDefault();
      this.splashScreen.hide();
      this._pushNotificationService.settingsInit();
      this.listenerOnLine();

    });

  }

  listenerOnLine(){

    this.platform.pause.subscribe(() => {

      this.user = JSON.parse(localStorage.getItem('userConnect'));

      if(this.user != null){

        this.user.onLine = false;

        this.updateOnLineById();

      }


    });

    this.platform.resume.subscribe(() => {

      this.user = JSON.parse(localStorage.getItem('userConnect'));

      if(this.user != null){

        this.user.onLine = true;

        this.updateOnLineById();

      }

    });

  }


  updateOnLineById(){

    this._userService.updateOnLineById(this.user).subscribe(res => {

      if(res.statusCode == 200){

      }else if(res.statusCode == 204){

      }else if(res.statusCode == 403){

      }else if(res.statusCode == 404){

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }

    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

}
