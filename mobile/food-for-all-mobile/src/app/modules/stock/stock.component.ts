import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NavController, ModalController, PopoverController, ActionSheetController } from '@ionic/angular';

import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';
import { CallNumber } from '@ionic-native/call-number/ngx';

import { StockService } from 'src/app/services/stock.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { StockCommentAddComponent } from '../stock-comment-add/stock-comment-add.component';
import { StockCommentService } from 'src/app/services/stock-comment.service';
import { StockLocationComponent } from '../stock-location/stock-location.component';
import { AlertService } from 'src/app/services/alert.service';
import { StockReceivedAddComponent } from '../stock-received-add/stock-received-add.component';
import { StockReceivedService } from 'src/app/services/stock-received.service';
import { StockCommentDeleteComponent } from '../stock-comment-delete/stock-comment-delete.component';
import { CalificationAddComponent } from '../calification-add/calification-add.component';
import { CalificationStockService } from 'src/app/services/calification-stock.service';
import { StockImageComponent } from '../stock-image/stock-image.component';
import { StockImageService } from 'src/app/services/stock-image.service';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.scss'],
  providers:[
    StockService,
    ToastService,
    StockService,
    AlertService,
    StockReceivedService,
    CalificationStockService,
    StockImageComponent,
  ]
})
export class StockComponent implements OnInit {

  stock:any = {};
  stockAvailable:any = {};
  calificationStock:any = {};

  user:any;

  stockComments: Array<any> = new Array<any>();
  stockReceiveds: Array<any> = new Array<any>();
  stockImages: Array<any> = new Array<any>();
  stars: Array<any> = new Array<any>();

  idStock: number;

  loadingFindById: boolean;

  constructor(private activatedRoute: ActivatedRoute, private _stockService: StockService, private _toastService: ToastService, private navController: NavController, private modalController: ModalController, private _stockCommentService: StockCommentService, private _alertService: AlertService, private _stockReceivedService: StockReceivedService, private popoverController: PopoverController, private photoViewer: PhotoViewer, private actionSheetController: ActionSheetController, private callNumber: CallNumber, private _calificationStockService: CalificationStockService, private _stockImageService: StockImageService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));

    this.idStock = parseInt(this.activatedRoute.snapshot.paramMap.get('id'));

    this.findById();
    this._stockCommentService.startConnectionSignalR();
    this._stockReceivedService.startConnectionSignalR();
    this.listenerSignalR();

  }

  getPhotoProfile(user:any){

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

  getPhotoComment(i: number){

    this.photoViewer.show('data:image/jpeg;base64,' + this.stockComments[i].comment);

  }

  getPhotoStock(i: number){

    this.photoViewer.show('data:image/jpeg;base64,' + this.stockImages[i].referenceImage);

  }

  findById(){

    this.loadingFindById = true;

    this._stockService.findById(this.idStock).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindById = false;
        this.stock = res.stock;
        this.stars = new Array(this.stock.star == null ? 0 : this.stock.star);

        this.findByIdAndAvailable();
        this.findByIdStock();
        this.findByIdStockReceived();
        this.findByIdStockAndIdUserCalification();
        this.findByIdStockImage();

      }else if(res.statusCode == 204){

        this.loadingFindById = false;
        this._toastService.present(res.message);
        this.stockComments = [];
        this.stockReceiveds = [];

      }else if(res.statusCode == 403){
        
        this.loadingFindById = false;
        this._alertService.present();

      }else if(res.statusCode == 404){
        
        this.loadingFindById = false;
        this.stock = null;

      }else if(res.statusCode == 500){
        
        this.loadingFindById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }
 
  findByIdStockAndIdUserCalification(){

    this._calificationStockService.findByIdStockAndIdUserCalification(this.idStock, this.user.id).subscribe(res => {

      if(res.statusCode == 200){

        this.calificationStock = res.calificationStock;

      }else if(res.statusCode == 204){

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 404){

        this.calificationStock = null;

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }

    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdStockImage(){

    this._stockImageService.findByIdStock(this.stock.id).subscribe(res => {

      if(res.statusCode == 200){

        this.stockImages = res.stockImages;

      }else if(res.statusCode == 204){

        this.stockImages = res.stockImages;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }


    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdAndAvailable(){

    this.loadingFindById = true;

    this._stockService.findByIdAndAvailable(this.idStock).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindById = false;
        this.stockAvailable = res.stock;

      }else if(res.statusCode == 204){

        this.loadingFindById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){
        
        this.loadingFindById = false;
        this._alertService.present();

      }else if(res.statusCode == 404){
        
        this.loadingFindById = false;
        this.stockAvailable = null;

      }else if(res.statusCode == 500){
        
        this.loadingFindById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdStock(){

    this._stockCommentService.findByIdStock(this.stock.id).subscribe(res => {

      if(res.statusCode == 200){

        this.stockComments = res.stockComments;

      }else if(res.statusCode == 204){

        this.stockComments = res.stockComments;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }


    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdStockReceived(){

    this._stockReceivedService.findByIdStock(this.stock.id).subscribe(res => {

      if(res.statusCode == 200){

        this.stockReceiveds = res.stockReceiveds;

      }else if(res.statusCode == 204){

        this.stockReceiveds = res.stockReceiveds;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }


    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  refresh(e:any){

    this.findById();
    e.detail.complete();

  }

  callDonor(){

    this.callNumber.callNumber(this.stock.idUserNavigation.phone, true).then(res => {

    });
    
  }

  destroyCalification(){

    this._calificationStockService.destroyById(this.calificationStock.id).subscribe(res => {

      if(res.statusCode == 200){

        this.findById();

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

  async createStockReceived(){

    const modal = await this.modalController.create({
      component: StockReceivedAddComponent,
      componentProps:{
        stock: this.stock
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findById();


  }

  async deleteByIdStockComment(i: number){

    const popover = await this.popoverController.create({
      component: StockCommentDeleteComponent,
      componentProps:{
        stockComment: this.stockComments[i]
      },
      translucent: true,
    });

    popover.present();

    const { data } = await popover.onDidDismiss();

    this.findByIdStock();
  }

  async getLocation(){

    const modal = await this.modalController.create({
      component: StockLocationComponent,
      componentProps:{
        stock: this.stock
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findByIdStock();


  }

  async addCommentary(){

    const modal = await this.modalController.create({
      component: StockCommentAddComponent,
      componentProps:{
        stock: this.stock,
        playerIds: [...new Set(this.stockComments.map(sc => sc.idUserNavigation.oneSignalPlayerId))]
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findByIdStock();

  }

  async addCalification(){

    const modal = await this.popoverController.create({
      component: CalificationAddComponent,
      componentProps:{
        stock: this.stock,
        user: null
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findById();

  }

  async presentActionSheet(){

    const actionSheet = await this.actionSheetController.create({

      buttons:[
        {
          text: 'Llamada Whatsapp',
          icon: 'logo-whatsapp',
          handler: () =>{
            
            let a = document.createElement('a');
            a.href = 'https://api.whatsapp.com/send?phone=56' + this.stock.idUserNavigation.phone;
            a.target = '_blank';
            a.click();

          }
        },
        {
          text: 'Llamada Normal',
          icon: 'call',
          handler: () =>{
            
            this.callDonor();

          }
        }
      ]

    });

    await actionSheet.present();

  }

  async presentActionSheetCommeny(){

    const actionSheet = await this.actionSheetController.create({

      buttons:[
        {
          text: 'Mensaje por Whatsapp',
          icon: 'logo-whatsapp',
          handler: () =>{
            
            let a = document.createElement('a');
            a.href = 'https://api.whatsapp.com/send?phone=56' + this.stock.idUserNavigation.phone;
            a.target = '_blank';
            a.click();

          }
        },
        {
          text: 'Mensaje Privado',
          icon: 'chatbubbles',
          handler: () =>{
            
            this.navController.navigateForward('/message/' + this.user.id + '/' + this.stock.idUser);

          }
        },
        {
          text: 'Agregar Comentario',
          icon: 'chatboxes',
          handler: () =>{
            
            this.addCommentary();

          }
        }
      ]

    });

    await actionSheet.present();

  }

  listenerSignalR(){

    this._stockCommentService.hubConnection.on('create', res =>{

      if(res.idUser != this.user.id){

        this.stockComments.push(res);

        let message: string = 'Nuevo comentario agregado.';

        this._toastService.present(message);

      }

    });

    this._stockReceivedService.hubConnection.on('create', res =>{

      this.ngOnInit();

    });

  }

}
