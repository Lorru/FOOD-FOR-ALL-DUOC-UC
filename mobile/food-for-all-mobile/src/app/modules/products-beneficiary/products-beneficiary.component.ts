import { Component, OnInit } from '@angular/core';

import { StockReceivedService } from 'src/app/services/stock-received.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-products-beneficiary',
  templateUrl: './products-beneficiary.component.html',
  styleUrls: ['./products-beneficiary.component.scss'],
  providers:[
    StockReceivedService,
    ToastService,
    AlertService
  ]
})
export class ProductsBeneficiaryComponent implements OnInit {

  user:any;

  productTypes: Array<any> = new Array<any>();
  stockReceiveds: Array<any> = new Array<any>();

  loadingFindByIdUserBeneficiary: boolean;

  searcher: string = null;

  constructor(private _stockReceivedService: StockReceivedService, private _toastService: ToastService, private _alertService: AlertService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));
    this.findByIdUserBeneficiary();

  }

  findByIdUserBeneficiary(){

    this.loadingFindByIdUserBeneficiary = true;

    this._stockReceivedService.findByIdUserBeneficiary(this.user.id, this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUserBeneficiary = false;
        this.stockReceiveds = res.stockReceiveds;

        this.stockReceiveds.forEach(stockReceived => {
          
          if(this.productTypes.find(pt => pt.id == stockReceived.idStockNavigation.idProductNavigation.idProductType) == null){

            this.productTypes.push(stockReceived.idStockNavigation.idProductNavigation.idProductTypeNavigation);

          }

        });

      }else if(res.statusCode == 204){

        this.loadingFindByIdUserBeneficiary = false;
        this.stockReceiveds = res.stockReceiveds;
        this.productTypes = [];

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUserBeneficiary = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUserBeneficiary = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUserBeneficiary = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });


  }

  refresh(e: any){

    this.productTypes = [];

    this.findByIdUserBeneficiary();
    e.detail.complete();


  }

  search(){

    this.productTypes = [];

    this.findByIdUserBeneficiary();

  }

}
