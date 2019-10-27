import { Component, OnInit } from '@angular/core';

import { NavController } from '@ionic/angular';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-recovery-password',
  templateUrl: './recovery-password.component.html',
  styleUrls: ['./recovery-password.component.scss'],
  providers:[
    UserService,
    ToastService
  ]
})
export class RecoveryPasswordComponent implements OnInit {

  user:any = {};

  loadingSendRecoveryPassword: boolean;

  constructor(private _userService: UserService, private _toastService: ToastService, private navController: NavController) { }

  ngOnInit() {

  }

  sendRecoveryPassword(){

    this.loadingSendRecoveryPassword = true;

    if(/^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i.test(this.user.email)){

      this._userService.sendRecoveryPassword(this.user).subscribe(res =>{

        if(res.statusCode == 200){
  
          this.loadingSendRecoveryPassword = false;
          this._toastService.present(res.message);
          this.navController.navigateForward('/set-password/' + res.token);
  
        }else if(res.statusCode == 204){
  
          this.loadingSendRecoveryPassword = false;
          this._toastService.present(res.message);
  
        }else if(res.statusCode == 404){
  
          this.loadingSendRecoveryPassword = false;
          this._toastService.present(res.message);
  
        }else if(res.statusCode == 500){
  
          this.loadingSendRecoveryPassword = false;
          this._toastService.present(res.message);
  
        }
  
      }, error => {

        console.log(error);
        this.loadingSendRecoveryPassword = false;
        this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);
  
      });

    }else{

      this.loadingSendRecoveryPassword = false;
      let message: string = 'Email no valido.';
      this._toastService.present(message);

    }

  }

}
