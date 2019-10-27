import { Component, OnInit, Input } from '@angular/core';

import { ModalController, NavController } from '@ionic/angular';

import { ToastService } from 'src/app/services/toast.service';
import { StockCommentService } from 'src/app/services/stock-comment.service';
import { environment } from 'src/environments/environment';
import { AlertService } from 'src/app/services/alert.service';
import { PushNotificationService } from 'src/app/services/push-notification.service';

import { Camera, CameraOptions } from '@ionic-native/camera/ngx';

@Component({
  selector: 'app-stock-comment-add',
  templateUrl: './stock-comment-add.component.html',
  styleUrls: ['./stock-comment-add.component.scss'],
  providers:[
    ToastService,
    StockCommentService,
    AlertService,
    PushNotificationService
  ]
})
export class StockCommentAddComponent implements OnInit {

  @Input() stock: any;
  @Input() playerIds: Array<string>;

  user:any;
  stockComment: any = {};
  notification: any = {};

  selectionComments: Array<any> = [
    { icon: 'create', color: 'primary'  },
    { icon: 'camera', color: 'dark'  },
    { icon: 'image',  color: 'dark'  }
  ];

  selectionComment: string = 'create';
  img: string = null;

  loadingCreate: boolean;

  constructor(private _stockCommentService: StockCommentService, private _toastService: ToastService, private modalController: ModalController, private navController: NavController, private _alertService: AlertService, private _pushNotificationService: PushNotificationService, private camera: Camera) { }

  ngOnInit() {
    
    this.user = JSON.parse(localStorage.getItem('userConnect'));

    this.stockComment.idTypeMessage = 1;

    this.playerIds.push(this.stock.idUserNavigation.oneSignalPlayerId);

    this.playerIds = this.playerIds.filter(p => p != this.user.oneSignalPlayerId);
  }

  create(){

    this.loadingCreate = true;
    this.stockComment.idStock = this.stock.id;
    this.stockComment.idUser = this.user.id;

    this._stockCommentService.create(this.stockComment).subscribe(res => {

      if(res.statusCode == 201){

        this.loadingCreate = false;
        this.sendNotification();
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 204){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingCreate = false;
        this._alertService.present();

      }else if(res.statusCode == 404){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }


    }, error => {

      this.loadingCreate = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  sendNotification(){

    this.notification.include_player_ids = this.playerIds;
    this.notification.data = {
      url: `/stock/${this.stock.id}`
    };
    this.notification.contents = {
      es: `"${this.stockComment.comment}"`,
      en: `"${this.stockComment.comment}"`
    };
    this.notification.headings = {
      es: `Nuevo comentario para ${this.stock.idProductNavigation.name}, de ${this.user.userName}.`,
      en: `Nuevo comentario para ${this.stock.idProductNavigation.name}, de ${this.user.userName}.`
    };

    this._pushNotificationService.sendNotification(this.notification).subscribe(res => {


    });

  }

  selectOptionComment(i:number){

    this.selectionComments.forEach(selectionComment => {
      
      selectionComment.color = 'dark';

    });

    this.selectionComment = this.selectionComments[i].icon;
    this.selectionComments[i].color = 'primary';

    if(this.selectionComment == 'create'){

      this.stockComment.comment = null;
      this.stockComment.idTypeMessage = 1;

    }else if(this.selectionComment == 'camera'){

      this.getCamera();

    }else if(this.selectionComment == 'image'){

      this.getPhotoLibrary();

    }

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

      this.img = res;
      this.stockComment.comment = this.img;
      this.stockComment.idTypeMessage = 2;

    });

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

      this.img = res;
      this.stockComment.comment = this.img;
      this.stockComment.idTypeMessage = 2;

    });

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
