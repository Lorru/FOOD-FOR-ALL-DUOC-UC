import { Component, OnInit, ViewChild } from '@angular/core';

import { IonSegment, NavController, ModalController } from '@ionic/angular';

import { Geolocation } from '@ionic-native/geolocation/ngx';

import { StockService } from 'src/app/services/stock.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';

import * as mapboxgl from 'mapbox-gl';
import { AlertService } from 'src/app/services/alert.service';
import { LocationService } from 'src/app/services/location.service';
import { StockReceivedAddComponent } from '../stock-received-add/stock-received-add.component';

@Component({
  selector: 'app-stock-searcher',
  templateUrl: './stock-searcher.component.html',
  styleUrls: ['./stock-searcher.component.scss'],
  providers:[
    StockService,
    ToastService,
    AlertService,
    LocationService
  ]
})
export class StockSearcherComponent implements OnInit {

  @ViewChild(IonSegment, {  static: true }) ionSegment: IonSegment;

  mapbox: any = {
    map: {},
    style: 'mapbox://styles/mapbox/streets-v11',
    latitude: 0,
    longitude: 0,
    
  };

  stocks: Array<any> = new Array<any>();
  productTypes: Array<any> = new Array<any>();
  locations: Array<any> = new Array<any>();

  loadingFindAll: boolean;
  loadingFindAllStockAvailable: boolean;
  loadingCreate: boolean;

  searcher: string = null;

  constructor(private _stockService: StockService, private _toastService: ToastService, private navController: NavController, private modalController: ModalController, private _alertService: AlertService, private _locationService: LocationService, private geolocation: Geolocation) { }

  ngOnInit() {

    this.ionSegment.value = 'search';
    this.findAllAvailable();
    this._stockService.startConnectionSignalR();
    this.listenerSignalR();

  }

  findAllAvailable(){

    this.loadingFindAll = true;

    this._stockService.findAllAvailable(this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAll = false;
        this.stocks = res.stocks;

        this.stocks.forEach(stock => {
          
          if(this.productTypes.find(pt => pt.id == stock.idProductNavigation.idProductType) == null){

            this.productTypes.push(stock.idProductNavigation.idProductTypeNavigation);

          }

        });

      }else if(res.statusCode == 204){

        this.loadingFindAll = false;
        this.stocks = res.stocks;
        this.productTypes = [];

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindAll = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindAll = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindAll = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findAllStockAvailable(){

    this.loadingFindAllStockAvailable = true;

    this._locationService.findAllStockAvailable().subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAllStockAvailable = false;
        this.locations = res.locations;
        this.getCurrentPosition();

      }else if(res.statusCode == 204){

        this.loadingFindAllStockAvailable = false;
        this.locations = res.locations;

      }else if(res.statusCode == 403){

        this.loadingFindAllStockAvailable = false;
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindAllStockAvailable = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindAllStockAvailable = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  search(){

    this.productTypes = [];

    this.findAllAvailable();

  }

  changeIonSegment(e: any){

    if(e.detail.value == 'All'){

      this.findAllAvailable();

    }else{

      this.findAllStockAvailable();

    }

  }

  refresh(e: any){

    this.productTypes = [];
    this.findAllAvailable();
    e.detail.complete();

  }

  getCurrentPosition(){
    
    this.geolocation.getCurrentPosition().then((res) => {


      this.mapbox.latitude = res.coords.latitude;
      this.mapbox.longitude = res.coords.longitude;

      setTimeout(() => {
        
        this.getLocation();

      }, 1000);


     }).catch((error) => {


      this.mapbox.latitude = 0;
      this.mapbox.longitude = 0;

      setTimeout(() => {
        
        this.getLocation();

      }, 1000);

     });

  }

  getLocation(){

    let container = document.getElementById('map');

    if(container){

      let self = this;

      mapboxgl.accessToken = environment.MAPBOX_TOKEN;
  
      this.mapbox.map = new mapboxgl.Map({
        container: container, 
        style: this.mapbox.style, 
        center: [
          this.mapbox.longitude, 
          this.mapbox.latitude
        ], 
        zoom: 13
      });
  
      this.mapbox.map.on('load', function(){
  
        self.locations.forEach(location => {
          
          let popup = new mapboxgl.Popup({ offset: 25 }).setHTML(`<ul><li>Dirección: ${location.address}</li><li>País: ${location.country}</li><li>Donador: ${location.idUserNavigation.userName}</li><li>Teléfono: ${location.idUserNavigation.phone}</li><li><a href="/stock-available-donor/${location.idUser}">Ver Stock</a></li></ul>`);
  
          new mapboxgl.Marker({draggable: false}).setLngLat([location.longitude, location.latitude]).setPopup(popup).addTo(self.mapbox.map);

        });
    
        self.mapbox.map.resize();
  
      });
      

    }
    
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

  listenerSignalR(){

    this._stockService.hubConnection.on('create', res =>{

      this.ngOnInit();

    });

  }

}
