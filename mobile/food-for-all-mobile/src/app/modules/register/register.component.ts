import { Component, OnInit } from '@angular/core';

import { NavController, ModalController } from '@ionic/angular';

import { Facebook, FacebookLoginResponse } from '@ionic-native/facebook/ngx';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { PushNotificationService } from 'src/app/services/push-notification.service';
import { SelectInstitutionComponent } from '../select-institution/select-institution.component';
import { InstitutionService } from 'src/app/services/institution.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  providers:[
    UserService,
    ToastService,
    PushNotificationService,
    InstitutionService
  ]
})
export class RegisterComponent implements OnInit {

  user: any = {};
  institution: any = {};

  institutions: Array<any> = new Array<any>();

  typePassword: string = 'password';
  iconPassword: string = 'eye-off';
  imei: string = null;

  userType: boolean = true;
  loadingCreate: boolean;
  loadingFindAll: boolean;

  constructor(private _userService: UserService, private _toastService: ToastService, private navController: NavController, private facebook: Facebook, private _pushNotificationService: PushNotificationService, private modalController: ModalController, private _institutionService: InstitutionService) { }

  ngOnInit() {

    this._pushNotificationService.getPlayerId();
    this.findAll();

  }

  showHidePassword(){

    this.typePassword = this.typePassword === 'text' ? 'password' : 'text';
    this.iconPassword = this.iconPassword === 'eye-off' ? 'eye' : 'eye-off';

  }

  create(){

    this.user.idUserType = this.userType == true ? 2 : 3;
    this.user.isWithFacebook = false;
    this.user.oneSignalPlayerId = this._pushNotificationService.playerId;
    this.user.imei = this.imei;

    if(this.user.idUserType == 3){

      this.user.idInstitution = this.institution.id;

    }else{

      this.user.idInstitution = null;

    }

    this.loadingCreate = true;
    
    if(!/^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i.test(this.user.email)){

      this.loadingCreate = false;
      let message: string = 'Email no valido.';
      this._toastService.present(message);

      return;
    }

    if(this.user.password != null && this.user.password.length < 4){

      this.loadingCreate = false;
      let message: string = 'La Clave debe contener mas de 4 caracteres.';
      this._toastService.present(message);

      return;

    }

    this._userService.create(this.user).subscribe(res => {

      if(res.statusCode == 201){

        this.loadingCreate = false;
        localStorage.setItem('token', res.token);
        localStorage.setItem('userConnect', JSON.stringify(res.userCreated));
        this.navController.navigateForward('/menu/home');

      }else if(res.statusCode == 204){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 404){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 409){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }

    },error => {

      this.loadingCreate = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  createWithFacebook(user:any){

    this.loadingCreate = true;

    this._userService.create(user).subscribe(res => {

      if(res.statusCode == 201){

        this.loadingCreate = false;
        localStorage.setItem('token', res.token);
        localStorage.setItem('userConnect', JSON.stringify(res.userCreated));
        this.navController.navigateForward('/menu/home');

      }else if(res.statusCode == 204){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 404){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 409){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }

    },error => {

      this.loadingCreate = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  loginWithFacebook(){

    this.facebook.login(['public_profile', 'email']).then((res: FacebookLoginResponse) => {

      if(res.status == 'connected'){

        this.getInformationFacebook();

      }

    });

  }

  getInformationFacebook(){

    this.facebook.api('/me?fields=id,email,picture.type(large)',['public_profile','email']).then(res => {

      let user: any = {};

      user.email = res.email;
      user.userName = res.email.split('@')[0];
      user.photo = res.picture.data.url;
      user.isWithFacebook = true;
      user.idUserType = 2;
      user.oneSignalPlayerId = this._pushNotificationService.playerId;
      this.user.imei = this.imei;

      this.createWithFacebook(user);

    });

  }

  findAll(){

    this.loadingFindAll = true;

    this._institutionService.findAll().subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAll = false;
        this.institutions = res.institutions;
        this.institution = this.institutions[0];

      }else if(res.statusCode == 204){

        this.loadingFindAll = false;
        this.institutions = res.institutions;
        this.institution = {};

      }else if(res.statusCode == 500){

        this.loadingFindAll = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindAll = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  async selectInstitution(){

    const modal = await this.modalController.create({
      component: SelectInstitutionComponent,
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    if(data.ok == true){

      this.institution = data.institution;

    }

  }

}
