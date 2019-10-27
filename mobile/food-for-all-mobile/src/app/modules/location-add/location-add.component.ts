import { Component, OnInit, Input } from '@angular/core';

import { NavController, ModalController } from '@ionic/angular';

import { Geolocation } from '@ionic-native/geolocation/ngx';

import { LocationService } from 'src/app/services/location.service';
import { ToastService } from 'src/app/services/toast.service';

import * as mapboxgl from 'mapbox-gl';

import { environment } from 'src/environments/environment';
import { MapboxService } from 'src/app/services/mapbox.service';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-location-add',
  templateUrl: './location-add.component.html',
  styleUrls: ['./location-add.component.scss'],
  providers:[
    LocationService,
    ToastService,
    MapboxService,
    AlertService
  ]
})
export class LocationAddComponent implements OnInit {

  @Input() idUser: number;

  @Input() locations: Array<any>;

  locationsMapbox: Array<any> = new Array<any>();

  location:any = {};

  mapbox: any = {
    map: {},
    style: 'mapbox://styles/mapbox/streets-v11',
    latitude: 0,
    longitude: 0,
    address: '',
    country: ''
  };

  loadingCreate: boolean = false;
  currentPosition: boolean;

  searchAddress: string = null;

  constructor(private _locationService: LocationService, private _toastService: ToastService, private navController: NavController, private modalController: ModalController, private geolocation: Geolocation, private _mapboxService: MapboxService, private _alertService: AlertService) { }

  ngOnInit() {

    if(this.locations.length > 0){

      this.locations = this.locations.filter(l => l.isMain);

    }

    this.location.idUser = this.idUser;
    this.location.isMain = false;

    this.getLocationDefault();

  }

  getLocationDefault(){

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

        let popup = new mapboxgl.Popup({ offset: 25 }).setHTML(`<ul><li>Dirección: ${self.mapbox.address}</li><li>País: ${self.mapbox.country}</li></ul>`);
  
        new mapboxgl.Marker({draggable: false}).setLngLat([self.mapbox.longitude, self.mapbox.latitude]).setPopup(popup).addTo(self.mapbox.map);
    
        self.mapbox.map.resize();
  
      });

    }

  }

  getCurrentPosition(){
    
    this.geolocation.getCurrentPosition().then((res) => {


      this.mapbox.latitude = res.coords.latitude;
      this.mapbox.longitude = res.coords.longitude;

      this.getLocationDefault();
      this.getAddressByLongitudeAndLatitude();


     }).catch((error) => {


      this.mapbox.latitude = 0;
      this.mapbox.longitude = 0;

      this.currentPosition = false;

      this.getLocationDefault();

     });

  }

  getAddressByLongitudeAndLatitude(){

    this._mapboxService.getAddressByLongitudeAndLatitude(this.mapbox.longitude, this.mapbox.latitude).subscribe(res => {

      this.mapbox.address = res.features[0].place_name;
      this.mapbox.country = res.features[5].place_name

    });

  }

  selectAddress(i: number){

    this.searchAddress = this.locationsMapbox[i].address;
    this.mapbox.longitude = this.locationsMapbox[i].coords[0];
    this.mapbox.latitude = this.locationsMapbox[i].coords[1];
    this.mapbox.address = this.searchAddress;
    this.mapbox.country = this.locationsMapbox[i].country;
    this.locationsMapbox = [];
    this.getLocationDefault();

  }

  getAddressBySearch(){

    this._mapboxService.getAddressBySearch(this.searchAddress).subscribe(res => {

      if(res.features.length > 0){

        let locationMapbox: any = {
          address: res.features[0].place_name,
          country: res.features[0].place_name.split(',').pop().split(' ')[1],
          coords: res.features[0].center
        }
  
        this.locationsMapbox.push(locationMapbox);
  
        if(this.locationsMapbox.length > 3){
  
          this.locationsMapbox = [];
  
        }

      }

    });

  }

  create(){

    this.loadingCreate = true;
    this.location.longitude = this.mapbox.longitude;
    this.location.latitude = this.mapbox.latitude;
    this.location.address = this.mapbox.address;
    this.location.country = this.mapbox.country;

    this._locationService.create(this.location).subscribe(res => {

      if(res.statusCode == 201){

        this.loadingCreate = false;
        this._toastService.present(res.message);
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 204){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingCreate = false;
        this.dismiss();
        this._alertService.present();

      }else if(res.statusCode == 404){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 409){

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
