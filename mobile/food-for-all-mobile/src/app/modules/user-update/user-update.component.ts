import { Component, OnInit, Input } from '@angular/core';

import { ModalController } from '@ionic/angular';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment.prod';
import { PushNotificationService } from 'src/app/services/push-notification.service';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.scss'],
  providers:[
    UserService,
    ToastService,
    AlertService,
    PushNotificationService
  ]
})
export class UserUpdateComponent implements OnInit {

  @Input() user: any;

  loadingUpdateById: boolean;

  typePassword: string = 'password';
  iconPassword: string = 'eye-off';

  constructor(private _userService: UserService, private _toastService: ToastService, private _alertService: AlertService, private modalController: ModalController, private _pushNotificationService: PushNotificationService) { }

  ngOnInit() {
    
    this.user.password = null;
    this._pushNotificationService.getPlayerId();

  }

  showHidePassword(){

    this.typePassword = this.typePassword === 'text' ? 'password' : 'text';
    this.iconPassword = this.iconPassword === 'eye-off' ? 'eye' : 'eye-off';

  }

  updateById(){

    this.loadingUpdateById = true;
    this.user.oneSignalPlayerId = this._pushNotificationService.playerId;

    if(!/^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i.test(this.user.email)){

      this.loadingUpdateById = false;
      let message: string = 'Email no valido.';
      this._toastService.present(message);

      return;
    }

    if(this.user.phone != null && this.user.phone != ''){

      if(!/^(9)(\s?)[987654]\d{7}$/.test(this.user.phone)){

        this.loadingUpdateById = false;
        let message: string = 'Teléfono no valido.';
        this._toastService.present(message);
  
        return;
      }

    }

    this._userService.updateById(this.user).subscribe(res =>{

      if(res.statusCode == 200){

        if(this.user.password == null || this.user.password == ''){

          this.loadingUpdateById = false;
          this._toastService.present(res.message);
          this.modalController.dismiss({ok:true});

        }else{

          this.updatePasswordById();

        }

      }else if(res.statusCode == 204){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingUpdateById = false;
        this._alertService.present();
        this.modalController.dismiss({ok:false});

      }else if(res.statusCode == 404){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingUpdateById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  updatePasswordById(){

    this.loadingUpdateById = true;

    if(this.user.password != null && this.user.password.length < 4){

      this.loadingUpdateById = false;
      let message: string = 'La Clave debe contener mas de 4 caracteres.';
      this._toastService.present(message);

      return;

    }

    this._userService.updatePasswordById(this.user).subscribe(res =>{

      if(res.statusCode == 200){

        this.loadingUpdateById = false;
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 204){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingUpdateById = false;
        this._alertService.present();
        this.modalController.dismiss({ok:false});

      }else if(res.statusCode == 404){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingUpdateById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
