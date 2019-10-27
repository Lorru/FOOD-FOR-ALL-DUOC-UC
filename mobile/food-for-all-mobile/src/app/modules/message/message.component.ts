import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'src/app/services/message.service';
import { AlertService } from 'src/app/services/alert.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { PushNotificationService } from 'src/app/services/push-notification.service';
import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { IonContent } from '@ionic/angular';
import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
  providers:[
    MessageService,
    AlertService,
    ToastService,
    PushNotificationService,
    UserService
  ]
})
export class MessageComponent implements OnInit {

  @ViewChild(IonContent, {  static: true }) ionContent: IonContent;

  userConnect: any = {};
  userReceived: any = {};
  message: any = {};
  notification: any = {};

  messages: Array<any> = new Array<any>();

  idUserSend: number;
  idUserReceived: number;

  loadingFindByIdUserSendAndIdUserReceived: boolean;
  onLineUserReceived: boolean;

  messageString: string;
  userNameUserReceived: string;

  constructor(private activatedRoute: ActivatedRoute, private _messageService: MessageService, private _toastService: ToastService, private _alertService: AlertService, private _pushNotificationService: PushNotificationService, private camera: Camera, private photoViewer: PhotoViewer, private _userService: UserService) { }

  ngOnInit() {

    this.userConnect = JSON.parse(localStorage.getItem('userConnect'));
    this.idUserSend = parseInt(this.activatedRoute.snapshot.paramMap.get('idUserSend'));
    this.idUserReceived = parseInt(this.activatedRoute.snapshot.paramMap.get('idUserReceived'));

    this.findByIdUserSendAndIdUserReceived();
    this._messageService.startConnectionSignalR();
    this._userService.startConnectionSignalR();
    this.listenerSignalR();

    setTimeout(() => {
          
      this.ionContent.scrollToBottom(300);

    }, 5000);

  }

  findByIdUserSendAndIdUserReceived(){

    this.loadingFindByIdUserSendAndIdUserReceived = true;

    this._messageService.findByIdUserSendAndIdUserReceived(this.idUserSend, this.idUserReceived).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUserSendAndIdUserReceived = false;
        this.messages = res.messages;
        this.userReceived = this.userConnect.id == res.userReceived.id ? res.userSend : res.userReceived;
        this.userNameUserReceived = this.userReceived.userName;
        this.onLineUserReceived = this.userReceived.onLine;

      }else if(res.statusCode == 204){

        this.loadingFindByIdUserSendAndIdUserReceived = false;
        this.messages = res.messages;
        this.userReceived = this.userConnect.id == res.userReceived.id ? res.userSend : res.userReceived;
        this.userNameUserReceived = this.userReceived.userName;
        this.onLineUserReceived = this.userReceived.onLine;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUserSendAndIdUserReceived = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUserSendAndIdUserReceived = false;
        this._toastService.present(res.message);

      }


    }, error => {

      this.loadingFindByIdUserSendAndIdUserReceived = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  create(){

    this._messageService.create(this.message).subscribe(res => {

      if(res.statusCode == 201){

        let message: any = JSON.parse(JSON.stringify(this.message));
        message.date = new Date().toISOString();
        this.messages.push(message);
        this.messageString = null;
        this.ionContent.scrollToBottom(300);
        this.sendNotification();

      }else if(res.statusCode == 204){

        if(res.message){

           this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 404){

        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }

    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  getCamera(){

    const options: CameraOptions = {
      quality: 60,
      destinationType: this.camera.DestinationType.DATA_URL,
      encodingType: this.camera.EncodingType.JPEG,
      mediaType: this.camera.MediaType.PICTURE,
      correctOrientation: true,
      sourceType: this.camera.PictureSourceType.CAMERA
    }
    
    this.camera.getPicture(options).then(res => {

      this.message.idUserSend = this.userConnect.id;
      this.message.idUserReceived = this.userConnect.id == this.idUserReceived ? this.idUserSend : this.idUserReceived;
      this.message.idTypeMessage = 2;
      this.message.message1 = res;
  
      this.create();

    });

  }

  getPhoto(i: number){

    this.photoViewer.show('data:image/jpeg;base64,' + this.messages[i].message1);

  }

  getPhotoLibrary(){

    const options: CameraOptions = {
      quality: 60,
      destinationType: this.camera.DestinationType.DATA_URL,
      encodingType: this.camera.EncodingType.JPEG,
      mediaType: this.camera.MediaType.PICTURE,
      correctOrientation: true,
      sourceType: this.camera.PictureSourceType.PHOTOLIBRARY
    }
    
    this.camera.getPicture(options).then(res => {

      this.message.idUserSend = this.userConnect.id;
      this.message.idUserReceived = this.userConnect.id == this.idUserReceived ? this.idUserSend : this.idUserReceived;
      this.message.idTypeMessage = 2;
      this.message.message1 = res;
  
      this.create();

    });

  }

  sendNotification(){

    this.notification.include_player_ids = [this.userReceived.oneSignalPlayerId];
    this.notification.data = {
      url: `/message/${this.userConnect.id}/${this.userReceived.id}`
    };
    this.notification.contents = {
      es : this.message.idTypeMessage == 1 ? this.message.message1 : 'Foto',
      en : this.message.idTypeMessage == 1 ? this.message.message1 : 'Foto'
    };
    this.notification.headings = {
      es: 'Nuevo Mensaje de ' + this.userConnect.userName,
      en: 'Nuevo Mensaje de ' + this.userConnect.userName
    };

    this._pushNotificationService.sendNotification(this.notification).subscribe(res => {


    });

  }

  createMessage(){

    this.message.idUserSend = this.userConnect.id;
    this.message.idUserReceived = this.userConnect.id == this.idUserReceived ? this.idUserSend : this.idUserReceived;
    this.message.idTypeMessage = 1;
    this.message.message1 = this.messageString;

    this.create();

  }

  scroll(e: any){

    setTimeout(() => {
      
      this.findByIdUserSendAndIdUserReceived();
      e.target.complete();

    }, 1000);

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

  listenerSignalR(){

    this._messageService.hubConnection.on('create', res =>{

      
      if(res.idUserReceived == this.userConnect.id){

        this.messages.push(res);
        this.ionContent.scrollToBottom(300);

      }

    });

    this._userService.hubConnection.on('findByUserNameAndPassword', res => {

      if(res.id == this.userReceived.id){

        this.onLineUserReceived = true;

      }

    });

    this._userService.hubConnection.on('findByEmailWithFacebook', res => {

      if(res.id == this.userReceived.id){

        this.onLineUserReceived = true;

      }

    });

    this._userService.hubConnection.on('updateOnLineById', res => {

      if(res.id == this.userReceived.id){

        this.onLineUserReceived = res.onLine;

      }

    });

  }
}
