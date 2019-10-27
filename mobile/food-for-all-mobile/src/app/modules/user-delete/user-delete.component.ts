import { Component, OnInit, Input } from '@angular/core';

import { ModalController, NavController } from '@ionic/angular';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.scss'],
  providers:[
    UserService,
    ToastService,
    AlertService
  ]
})
export class UserDeleteComponent implements OnInit {

  @Input() user: any;

  loadingDeleteById: boolean;

  constructor(private _userService: UserService, private _toastService: ToastService, private _alertService: AlertService, private modalController: ModalController, private navController: NavController) { }

  ngOnInit() {}

  deleteById(){

    this.loadingDeleteById = true;

    this._userService.deleteById(this.user.id).subscribe(res =>{

      if(res.statusCode == 200){

        this.loadingDeleteById = false;
        this._toastService.present(res.message);
        this.modalController.dismiss({ok:true});
        this.navController.navigateForward('/');

      }else if(res.statusCode == 204){

        this.loadingDeleteById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingDeleteById = false;
        this._alertService.present();
        this.modalController.dismiss({ok:false});

      }else if(res.statusCode == 404){

        this.loadingDeleteById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingDeleteById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingDeleteById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });


  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }
}
