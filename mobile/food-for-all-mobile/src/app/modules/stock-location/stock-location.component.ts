import { Component, OnInit, Input } from '@angular/core';

import { ModalController, NavController } from '@ionic/angular';

import { environment } from 'src/environments/environment';
import { LocationService } from 'src/app/services/location.service';
import { ToastService } from 'src/app/services/toast.service';

import * as mapboxgl from 'mapbox-gl';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-stock-location',
  templateUrl: './stock-location.component.html',
  styleUrls: ['./stock-location.component.scss'],
  providers:[
    LocationService,
    ToastService,
    AlertService
  ]
})
export class StockLocationComponent implements OnInit {

  @Input() stock: any;

  user:any;

  location: any = {};

  mapbox: any = {
    map: {},
    style: 'mapbox://styles/mapbox/streets-v11',
    latitude: 0,
    longitude: 0,
    
  };

  loadingFindByIdUserAndMain: boolean;

  constructor(private modalController: ModalController, private _locationService: LocationService, private _toastService: ToastService, private navController: NavController, private _alertService: AlertService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));

    this.findByIdUserAndMain();

  }

  findByIdUserAndMain(){

    this.loadingFindByIdUserAndMain = true;

    this._locationService.findByIdUserAndMain(this.stock.idUser).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUserAndMain = false;
        this.location = res.location;
        this.mapbox.latitude = this.location.latitude;
        this.mapbox.longitude = this.location.longitude;
        this.mapbox.address = this.location.address;
        this.mapbox.country = this.location.country;

        setTimeout(() => {
          
          this.getLocation();

        }, 1000);

      }else if(res.statusCode == 204){

        this.loadingFindByIdUserAndMain = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){
        
        this.loadingFindByIdUserAndMain = false;
        this._alertService.present();

      }else if(res.statusCode == 404){
        
        this.loadingFindByIdUserAndMain = false;
        this.location = null;

      }else if(res.statusCode == 500){
        
        this.loadingFindByIdUserAndMain = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUserAndMain = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

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

        let popup: any;

        if(self.stock.idUser == self.user.id){

          popup = new mapboxgl.Popup({ offset: 25 }).setHTML(`<ul><li>Dirección: ${self.mapbox.address}</li><li>País: ${self.mapbox.country}</li></ul>`);

        }else{

          popup = new mapboxgl.Popup({ offset: 25 }).setHTML(`<ul><li>Dirección: ${self.mapbox.address}</li><li>País: ${self.mapbox.country}</li><li>Donador: ${self.stock.idUserNavigation.userName}</li><li>Telefonó: ${self.stock.idUserNavigation.phone ? self.stock.idUserNavigation.phone : ''}</li></ul>`);

        }
  
        new mapboxgl.Marker({draggable: false}).setLngLat([self.mapbox.longitude, self.mapbox.latitude]).setPopup(popup).addTo(self.mapbox.map);
    
        self.mapbox.map.resize();
  
      });

    }

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
