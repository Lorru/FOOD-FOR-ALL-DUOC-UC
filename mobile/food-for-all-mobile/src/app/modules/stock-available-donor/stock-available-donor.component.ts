import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';

import { StockService } from 'src/app/services/stock.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';
import { ModalController } from '@ionic/angular';
import { StockReceivedAddComponent } from '../stock-received-add/stock-received-add.component';

@Component({
  selector: 'app-stock-available-donor',
  templateUrl: './stock-available-donor.component.html',
  styleUrls: ['./stock-available-donor.component.scss'],
  providers:[
    StockService,
    ToastService,
    AlertService
  ]
})
export class StockAvailableDonorComponent implements OnInit {

  stocks: Array<any> = new Array<any>();
  productTypes: Array<any> = new Array<any>();

  loadingFindByIdUserDonor: boolean;

  idUser: number;

  searcher: string = null;
  donor: string = null;

  constructor(private _stockService: StockService, private _toastService: ToastService, private _alertService: AlertService, private activatedRoute: ActivatedRoute, private modalController: ModalController) { }

  ngOnInit() {
    
    this.idUser = parseInt(this.activatedRoute.snapshot.paramMap.get('id'));

    this.findByIdUserAndAvailable();

  }

  findByIdUserAndAvailable(){

    this.loadingFindByIdUserDonor = true;

    this._stockService.findByIdUserAndAvailable(this.idUser, this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUserDonor = false;

        this.stocks = res.stocks;
        this.donor = this.stocks[0].idUserNavigation.userName;

        this.stocks.forEach(stock => {
        
          if(this.productTypes.find(pt => pt.id == stock.idProductNavigation.idProductType) == null){

            this.productTypes.push(stock.idProductNavigation.idProductTypeNavigation);

          }

        });



      }else if(res.statusCode == 204){

        this.loadingFindByIdUserDonor = false;
        this.stocks = res.stocks;
        this.productTypes = [];


        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUserDonor = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUserDonor = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUserDonor = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  refresh(e: any){

    this.productTypes = [];
    this.findByIdUserAndAvailable();
    e.detail.complete();
  }

  search(){

    this.productTypes = [];
    this.findByIdUserAndAvailable();

  }

  async createStockReceived(i: number){

    const modal = await this.modalController.create({
      component: StockReceivedAddComponent,
      componentProps:{
        stock: this.stocks[i]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();


  }
}
