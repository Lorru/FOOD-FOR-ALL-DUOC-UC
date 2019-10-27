import { Component, OnInit, ViewChild } from '@angular/core';

import { IonSegment, NavController, ModalController } from '@ionic/angular';

import { StockService } from 'src/app/services/stock.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { StockAddComponent } from '../stock-add/stock-add.component';
import { StockUpdateComponent } from '../stock-update/stock-update.component';
import { StockObservationComponent } from '../stock-observation/stock-observation.component';
import { AlertService } from 'src/app/services/alert.service';
import { StockAvailableAddComponent } from '../stock-available-add/stock-available-add.component';
import { StockAvailableDestroyComponent } from '../stock-available-destroy/stock-available-destroy.component';
import { StockImageComponent } from '../stock-image/stock-image.component';
import { StockReceivedService } from 'src/app/services/stock-received.service';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.scss'],
  providers:[
    StockService,
    ToastService,
    AlertService,
    StockReceivedService
  ]
})
export class StocksComponent implements OnInit {

  @ViewChild(IonSegment, {  static: true }) ionSegment: IonSegment;

  user:any = {};

  stocks: Array<any> = new Array<any>();
  stocksAvailable: Array<any> = new Array<any>();
  productTypes: Array<any> = new Array<any>();

  loadingFindByIdUser: boolean;

  searcher: string = null;

  constructor(private _stockService: StockService, private _toastService: ToastService, private navController: NavController, private modalController: ModalController, private _alertService: AlertService, private _stockReceivedService: StockReceivedService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));
    this.ionSegment.value = 'All';
    this.findByIdUser();
    this._stockReceivedService.startConnectionSignalR();
    this.listenerSignalR();

  }

  findByIdUser(){

    this.loadingFindByIdUser = true;

    this._stockService.findByIdUser(this.user.id, this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUser = false;
        this.stocks = res.stocks;
        this.findByIdUserAndAvailable();

        this.stocks.forEach(stock => {
          
          if(this.productTypes.find(pt => pt.id == stock.idProductNavigation.idProductType) == null){

            this.productTypes.push(stock.idProductNavigation.idProductTypeNavigation);

          }

        });

      }else if(res.statusCode == 204){

        this.loadingFindByIdUser = false;
        this.stocks = res.stocks;
        this.productTypes = [];

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUser = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUser = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUser = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdUserAndAvailable(){

    this.loadingFindByIdUser = true;

    this._stockService.findByIdUserAndAvailable(this.user.id, this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUser = false;

        this.stocksAvailable = res.stocks;

        if(this.ionSegment.value == 'Available'){

          this.stocks = res.stocks;

          this.stocks.forEach(stock => {
          
            if(this.productTypes.find(pt => pt.id == stock.idProductNavigation.idProductType) == null){
  
              this.productTypes.push(stock.idProductNavigation.idProductTypeNavigation);
  
            }
  
          });

        }



      }else if(res.statusCode == 204){

        this.loadingFindByIdUser = false;
        
        if(this.ionSegment.value == 'Available'){

          this.stocks = res.stocks;
          this.productTypes = [];
        }

        this.stocksAvailable = res.stocks;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUser = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUser = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUser = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdUserFilterDynamic(property: string, condition: string, value: string){

    this.loadingFindByIdUser = true;

    this._stockService.findByIdUserAndFilterDynamic(this.user.id, property, condition, value, this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUser = false;
        this.stocks = res.stocks;
        this.findByIdUserAndAvailable();

        this.stocks.forEach(stock => {
          
          if(this.productTypes.find(pt => pt.id == stock.idProductNavigation.idProductType) == null){

            this.productTypes.push(stock.idProductNavigation.idProductTypeNavigation);

          }

        });

      }else if(res.statusCode == 204){

        this.loadingFindByIdUser = false;
        this.stocks = res.stocks;
        this.productTypes = [];

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdUser = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdUser = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUser = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  changeIonSegment(e: any){

    this.productTypes = [];

    if(e.detail.value == 'All'){

      this.findByIdUser();

    }else{

      if(e.detail.value == 'IsAvailable'){

        this.findByIdUserFilterDynamic(e.detail.value, '==', 'true');

      }else if(e.detail.value == 'Quantity'){

        this.findByIdUserFilterDynamic(e.detail.value, '<', '2');

      }else if(e.detail.value == 'Status'){

        this.findByIdUserFilterDynamic(e.detail.value, '==', 'false');

      }else if(e.detail.value == 'Available'){

        this.findByIdUserAndAvailable();

      }

    }

  }

  refresh(e: any){

    this.productTypes = [];

    if(this.ionSegment.value == 'All'){

      this.findByIdUser();
      e.detail.complete();

    }else{

      if(this.ionSegment.value == 'IsAvailable'){

        this.findByIdUserFilterDynamic(this.ionSegment.value, '==', 'true');
        e.detail.complete();

      }else if(this.ionSegment.value == 'Quantity'){

        this.findByIdUserFilterDynamic(this.ionSegment.value, '<', '2');
        e.detail.complete();

      }else if(this.ionSegment.value == 'Status'){

        this.findByIdUserFilterDynamic(this.ionSegment.value, '==', 'false');
        e.detail.complete();

      }else if(this.ionSegment.value == 'Available'){

        this.findByIdUserAndAvailable();
        e.detail.complete();

      }

    }


  }

  search(){

    this.productTypes = [];

    if(this.ionSegment.value == 'All'){

      this.findByIdUser();

    }else{

      if(this.ionSegment.value == 'IsAvailable'){

        this.findByIdUserFilterDynamic(this.ionSegment.value, '==', 'true');

      }else if(this.ionSegment.value == 'Quantity'){

        this.findByIdUserFilterDynamic(this.ionSegment.value, '<', '2');

      }else if(this.ionSegment.value == 'Status'){

        this.findByIdUserFilterDynamic(this.ionSegment.value, '==', 'false');

      }else if(this.ionSegment.value == 'Available'){

        this.findByIdUserAndAvailable();

      }

    }

  }

  updateStatusById(i: number, value: boolean){

    let stock = JSON.parse(JSON.stringify(this.stocks[i]));
    stock.status = value;

    if(!value){

      stock.isAvailable = false;
      this.updateByIdStock(stock);

    }else{

      stock.isAvailable = stock.quantity > 0 ? true : false;
      this.updateByIdStock(stock);

    }

    this._stockService.updateStatusById(stock).subscribe(res => {

      if(res.statusCode == 200){

        this._toastService.present(res.message);

        this.ngOnInit();

      }else if(res.statusCode == 204){

        this._toastService.present(res.message);

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

  updateIsAvailable(i: number){

    let stock = JSON.parse(JSON.stringify(this.stocks[i]));

    stock.isAvailable = true;

    this._stockService.updateById(stock).subscribe(res => {

      if(res.statusCode == 200){

        this._toastService.present(res.message);

        this.ngOnInit();

      }else if(res.statusCode == 204){

        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 404){

        this._toastService.present(res.message);

      }else if(res.statusCode == 409){

        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }


    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });
  }

  updateByIdStock(stock:any){

    this._stockService.updateById(stock).subscribe(res => {

      if(res.statusCode == 200){

        this._toastService.present(res.message);

      }else if(res.statusCode == 204){

        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 404){

        this._toastService.present(res.message);

      }else if(res.statusCode == 409){

        this._toastService.present(res.message);

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }


    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  validateIsAvailableStock(i: number): boolean{

    let stock: any = this.stocks[i];

    if(stock.status && stock.isAvailable && stock.quantity > 0 && this.stocksAvailable.find(s => s.id == stock.id) == null){

      return true;

    }else{

      return false;

    }

  }


  validateDestroyIsAvailableStock(i: number): boolean{

    let stock: any = this.stocks[i];

    if(this.stocksAvailable.find(s => s.id == stock.id) != null){

      return true;

    }else{

      return false;

    }

  }

  async createStockAvailable(i: number){

    const modal = await this.modalController.create({
      component: StockAvailableAddComponent,
      componentProps:{
        stock: this.stocks[i]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  async destroyStockAvailable(i: number){

    const modal = await this.modalController.create({
      component: StockAvailableDestroyComponent,
      componentProps:{
        stock: this.stocks[i]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  async add(){

    const modal = await this.modalController.create({
      component: StockAddComponent,
      componentProps:{
        idUser: this.user.id
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  async updateById(i: number){

    const modal = await this.modalController.create({
      component: StockUpdateComponent,
      componentProps:{
        stock: this.stocks[i]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  async addObservation(i: number){

    const modal = await this.modalController.create({
      component: StockObservationComponent,
      componentProps:{
        stock: this.stocks[i]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  async stockImage(i: number){

    const modal = await this.modalController.create({
      component: StockImageComponent,
      componentProps:{
        stock: this.stocks[i]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

  listenerSignalR(){

    this._stockReceivedService.hubConnection.on('create', res =>{

      if(res.idStockNavigation.idUser == this.user.id){

        this.ngOnInit();

      }

    });

  }
}
