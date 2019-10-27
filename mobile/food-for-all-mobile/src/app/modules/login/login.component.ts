import { Component, OnInit } from '@angular/core';

import { NavController } from '@ionic/angular';

import { Facebook, FacebookLoginResponse } from '@ionic-native/facebook/ngx';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers:[
    UserService,
    ToastService
  ]
})
export class LoginComponent implements OnInit {

  user: any = {};

  typePassword: string = 'password';
  iconPassword: string = 'eye-off';

  loadingFindByUserNameAndPassword: boolean;

  constructor(private _userService: UserService, private _toastService: ToastService, private navController:NavController, private facebook: Facebook) { }

  ngOnInit() {}

  showHidePassword(){

    this.typePassword = this.typePassword === 'text' ? 'password' : 'text';
    this.iconPassword = this.iconPassword === 'eye-off' ? 'eye' : 'eye-off';

  }

  findByUserNameAndPassword(){

    this.loadingFindByUserNameAndPassword = true;

    this._userService.findByUserNameAndPassword(this.user).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByUserNameAndPassword = false;
        localStorage.setItem('token', res.token);
        localStorage.setItem('userConnect', JSON.stringify(res.userExisting));
        this.navController.navigateForward('/menu/home');

      }else if(res.statusCode == 204){

        this.loadingFindByUserNameAndPassword = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingFindByUserNameAndPassword = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingFindByUserNameAndPassword = false;
        this._toastService.present(res.message);

      }

    },error => {

      this.loadingFindByUserNameAndPassword = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByEmailWithFacebook(){

    this.loadingFindByUserNameAndPassword = true;

    this._userService.findByEmailWithFacebook(this.user).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByUserNameAndPassword = false;

        localStorage.setItem('token', res.token);
        localStorage.setItem('userConnect', JSON.stringify(res.userExisting));
        this.navController.navigateForward('/menu/home');

      }else if(res.statusCode == 204){

        this.loadingFindByUserNameAndPassword = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingFindByUserNameAndPassword = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingFindByUserNameAndPassword = false;
        this._toastService.present(res.message);

      }

    },error => {

      this.loadingFindByUserNameAndPassword = false;
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

      this.user.email = res.email;

      this.findByEmailWithFacebook();

    });

  }
}
