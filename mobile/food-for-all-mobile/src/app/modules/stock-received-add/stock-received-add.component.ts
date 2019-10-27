import { Component, OnInit, Input } from '@angular/core';

import { ModalController } from '@ionic/angular';

import { StockReceivedService } from 'src/app/services/stock-received.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-stock-received-add',
  templateUrl: './stock-received-add.component.html',
  styleUrls: ['./stock-received-add.component.scss'],
  providers:[
    StockReceivedService,
    ToastService,
    AlertService
  ]
})
export class StockReceivedAddComponent implements OnInit {

  @Input() stock: any;

  user:any;
  stockReceived: any = {};
  notification: any = {};

  quantities: Array<number> = new Array<number>();

  loadingCreate: boolean;

  constructor(private modalController: ModalController, private _stockReceivedService: StockReceivedService, private _toastService: ToastService, private _alertService: AlertService) { }

  ngOnInit() {
    
    this.user = JSON.parse(localStorage.getItem('userConnect'));

    for (let i = 0; i < this.stock.quantity; i++) {
      this.quantities.push(i);
      
    }

    this.stockReceived.quantity = this.quantities[0] + 1;

  }

  create(){

    this.loadingCreate = true;

    this.stockReceived.idUserBeneficiary = this.user.id
    this.stockReceived.idStock = this.stock.id;
    
    this._stockReceivedService.create(this.stockReceived).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingCreate = false;
        this._toastService.present(res.message);
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 201){

        this.loadingCreate = false;
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

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
