import { Component, OnInit, Input } from '@angular/core';

import { ModalController } from '@ionic/angular';

import { StockAvailableService } from 'src/app/services/stock-available.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';
import { PushNotificationService } from 'src/app/services/push-notification.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-stock-available-add',
  templateUrl: './stock-available-add.component.html',
  styleUrls: ['./stock-available-add.component.scss'],
  providers:[
    StockAvailableService,
    ToastService,
    AlertService,
    PushNotificationService,
    UserService
  ]
})
export class StockAvailableAddComponent implements OnInit {

  @Input() stock: any;

  user:any = {};
  stockAvailable: any = {}
  notification: any = {};
  users: Array<any> = new Array<any>();

  loadingCreate: boolean;
  loadingFindByIdUserAndFilterDynamic: boolean;

  constructor(private modalController: ModalController, private _stockAvailableService: StockAvailableService, private _toastService: ToastService, private _alertService: AlertService, private _pushNotificationService: PushNotificationService, private _userService: UserService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));
    this.findByIdUserAndFilterDynamic();

  }

  create(){

    this.loadingCreate = true;

    this.stockAvailable.idStock = this.stock.id;
    
    this._stockAvailableService.create(this.stockAvailable).subscribe(res => {

      if(res.statusCode == 201){

        this.loadingCreate = false;
        this.sendNotification();
        this._toastService.present(res.message);
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

  findByIdUserAndFilterDynamic(){

    this.loadingFindByIdUserAndFilterDynamic = true;

    this._userService.findByIdUserAndFilterDynamic(this.user.id, 'IdUserType', '==', '3').subscribe(res => {

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

  sendNotification(){

    if(this.users.length > 0){

      this.notification.include_player_ids = this.users.map(u => u.oneSignalPlayerId);
      this.notification.data = {
        url: `/stock/${this.stock.id}`
      };
      this.notification.contents = {
        es: `Donador: ${this.stock.idUserNavigation.userName}, Alimento: ${this.stock.idProductNavigation.name}, Cantidad: ${this.stock.quantity}.`,
        en: `Donador: ${this.stock.idUserNavigation.userName}, Alimento: ${this.stock.idProductNavigation.name}, Cantidad: ${this.stock.quantity}.`,
      };
      this.notification.headings = {
        es: 'Nuevo alimento disponible.',
        en: 'Nuevo alimento disponible.'
      };

      this._pushNotificationService.sendNotification(this.notification).subscribe(res => {

  
      });

    }

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
