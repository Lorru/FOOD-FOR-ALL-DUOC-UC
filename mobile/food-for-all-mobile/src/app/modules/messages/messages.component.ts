import { Component, OnInit } from '@angular/core';

import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { MessageService } from 'src/app/services/message.service';
import { environment } from 'src/environments/environment';
import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';
import { ModalController } from '@ionic/angular';
import { SelectUserMessageComponent } from '../select-user-message/select-user-message.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
  providers:[
    ToastService,
    AlertService,
    MessageService
  ]
})
export class MessagesComponent implements OnInit {

  user:any = {};

  messages: Array<any> = new Array<any>();

  loadingFindByIdUserLast: boolean;

  constructor(private _messageService: MessageService, private _alertService: AlertService, private _toastService: ToastService, private photoViewer: PhotoViewer, private modalController: ModalController) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));

    this.findByIdUserLast();
    this._messageService.startConnectionSignalR();
    this.listenerSignalR();

  }

  findByIdUserLast(){

    this.loadingFindByIdUserLast = true;

    this._messageService.findByIdUserLast(this.user.id).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUserLast = false;
        this.messages = res.messages;

      }else if(res.statusCode == 204){

        this.loadingFindByIdUserLast = false;
        this.messages = res.messages;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUserLast = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUserLast = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUserLast = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  refresh(e: any){

    this.findByIdUserLast();
    e.detail.complete();

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

  async add(){

    const modal = await this.modalController.create({
      component: SelectUserMessageComponent,
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  listenerSignalR(){

    this._messageService.hubConnection.on('create', res =>{

      if(res.idUserReceived == this.user.id){

        this.ngOnInit();

      }

    });

  }

}
