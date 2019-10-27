import { Component, OnInit, Input } from '@angular/core';

import { ModalController, NavController } from '@ionic/angular';

import { StockService } from 'src/app/services/stock.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-stock-update',
  templateUrl: './stock-update.component.html',
  styleUrls: ['./stock-update.component.scss'],
  providers:[
    StockService,
    ToastService,
    AlertService
  ]
})
export class StockUpdateComponent implements OnInit {

  @Input() stock: any;

  loadingUpdateById: boolean;

  min: string = new Date().toISOString().split('T')[0];
  max: string = new Date(new Date().setFullYear(new Date().getFullYear() + 2)).toISOString().split('T')[0];

  constructor(private _stockService: StockService, private _toastService: ToastService, private modalController: ModalController, private navController: NavController, private _alertService: AlertService) { }

  ngOnInit() {}

  updateById(){

    this.loadingUpdateById = true;

    this.stock.isAvailable = this.stock.quantity == 0 || this.stock.quantity < 0  ? false : this.stock.isAvailable;

    this._stockService.updateById(this.stock).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 204){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingUpdateById = false;
        this._alertService.present();

      }else if(res.statusCode == 404){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 409){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingUpdateById = false;
        this._toastService.present(res.message);

      }


    }, error => {

      this.loadingUpdateById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }
}
