import { Component, OnInit } from '@angular/core';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { ModalController, NavController } from '@ionic/angular';
import { environment } from 'src/environments/environment';
import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';

@Component({
  selector: 'app-select-user-message',
  templateUrl: './select-user-message.component.html',
  styleUrls: ['./select-user-message.component.scss'],
  providers:[
    UserService,
    ToastService,
    AlertService
  ]
})
export class SelectUserMessageComponent implements OnInit {

  userConnect:any = {};

  users:Array<any> = new Array<any>();

  loadingFindByIdUserAndFilterDynamic: boolean;

  searcher:string = null;

  constructor(private _userService: UserService, private _toastService: ToastService, private _alertService: AlertService, private modalController: ModalController, private photoViewer: PhotoViewer, private navController: NavController) { }

  ngOnInit() {

    this.userConnect = JSON.parse(localStorage.getItem('userConnect'));

    this.findByIdUserAndFilterDynamic();

  }

  findByIdUserAndFilterDynamic(){

    this.loadingFindByIdUserAndFilterDynamic = true;

    this._userService.findByIdUserAndFilterDynamic(this.userConnect.id, 'Status', '==', 'True', this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUserAndFilterDynamic = false;
        this.users = res.users;

      }else if(res.statusCode == 204){

        this.loadingFindByIdUserAndFilterDynamic = false;
        this.users = res.users;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUserAndFilterDynamic = false;
        this._alertService.present();
        this.modalController.dismiss({ok:false});

      }else if(res.statusCode == 500){

        this.loadingFindByIdUserAndFilterDynamic = false;
        this._toastService.present(res.message);

      }


    }, error => {

      this.loadingFindByIdUserAndFilterDynamic = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  search(){

    this.findByIdUserAndFilterDynamic();

  }

  getPhotoProfile(user: any){

    if(user.photo == null){

      this.photoViewer.show('/assets/img/user.png');

    }else{

      if(user.isWithFacebook){

        this.photoViewer.show(user.photo);

      }else{

        this.photoViewer.show('data:image/jpeg;base64,' + user.photo);

      }

    }

  }

  refresh(e: any){

    this.findByIdUserAndFilterDynamic();
    e.detail.complete();

  }

  addMessage(i: number){

    this.navController.navigateForward('/message/' + this.userConnect.id + '/' + this.users[i].id);
    this.dismiss();

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
