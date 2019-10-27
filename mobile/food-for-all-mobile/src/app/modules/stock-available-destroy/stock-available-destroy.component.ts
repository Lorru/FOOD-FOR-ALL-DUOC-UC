import { Component, OnInit, Input } from '@angular/core';

import { ModalController } from '@ionic/angular';

import { StockAvailableService } from 'src/app/services/stock-available.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-stock-available-destroy',
  templateUrl: './stock-available-destroy.component.html',
  styleUrls: ['./stock-available-destroy.component.scss'],
  providers:[
    StockAvailableService,
    ToastService,
    AlertService
  ]
})
export class StockAvailableDestroyComponent implements OnInit {

  @Input() stock: any;

  loadingDestroyById: boolean;

  constructor(private modalController: ModalController, private _stockAvailableService: StockAvailableService, private _toastService: ToastService, private _alertService: AlertService) { }

  ngOnInit() {}

  destroyById(){

    this.loadingDestroyById = true;
    
    this._stockAvailableService.destroyByIdStock(this.stock.id).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingDestroyById = false;
        this._toastService.present(res.message);
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 204){

        this.loadingDestroyById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingDestroyById = false;
        this._alertService.present();

      }else if(res.statusCode == 404){

        this.loadingDestroyById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this.loadingDestroyById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingDestroyById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
