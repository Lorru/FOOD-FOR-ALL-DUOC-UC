import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ToastService } from 'src/app/services/toast.service';
import { UserService } from 'src/app/services/user.service';
import { NavController } from '@ionic/angular';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.scss'],
  providers:[
    ToastService,
    UserService
  ]
})
export class SetPasswordComponent implements OnInit {

  user:any = {}

  token:string;

  loadingUpdatePasswordByIdAndRecovery: boolean;
  loadingFindByToken: boolean;

  typePassword: string = 'password';
  iconPassword: string = 'eye-off';

  constructor(private activatedRoute: ActivatedRoute, private _userService: UserService, private _toastService: ToastService, private navController: NavController) { }

  ngOnInit() {

    this.token = this.activatedRoute.snapshot.paramMap.get('token').toString();

    this.findByIdToken();

  }

  showHidePassword(){

    this.typePassword = this.typePassword === 'text' ? 'password' : 'text';
    this.iconPassword = this.iconPassword === 'eye-off' ? 'eye' : 'eye-off';

  }

  findByIdToken(){

    this.loadingFindByToken = true;

    this._userService.findByToken(this.token).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByToken = false;
        this.user = res.user;
        this.user.password = null;

      }else if(res.statusCode == 404){

        this.loadingFindByToken = false;
        this._toastService.present(res.message);
        this.user = null;

      }else if(res.statusCode == 500){

        this.loadingFindByToken = false;
        this._toastService.present(res.message);

      }

    }, error => {

      console.log(error);
      this.loadingFindByToken = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  updatePasswordByIdAndRecovery(){

    this.loadingUpdatePasswordByIdAndRecovery = true;

    if(this.user.password != null && this.user.password.length < 4){

      this.loadingUpdatePasswordByIdAndRecovery = false;
      let message: string = 'La Clave debe contener mas de 4 caracteres.';
      this._toastService.present(message);

      return;

    }

    this._userService.updatePasswordByIdAndRecovery(this.user).subscribe(res =>{

      if(res.statusCode == 200){

        this.loadingUpdatePasswordByIdAndRecovery = false;
        this._toastService.present(res.message);

        localStorage.setItem('token', res.token);
        localStorage.setItem('userConnect', JSON.stringify(res.userExisting));
        this.navController.navigateForward('/menu/home');

      }else if(res.statusCode == 204){

        this.loadingUpdatePasswordByIdAndRecovery = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 404){

        this.loadingUpdatePasswordByIdAndRecovery = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingUpdatePasswordByIdAndRecovery = false;
        this._toastService.present(res.message);

      }


    },error => {

      this.loadingUpdatePasswordByIdAndRecovery = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

}
