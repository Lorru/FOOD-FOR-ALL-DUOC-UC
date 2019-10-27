import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NavController } from '@ionic/angular';

import { LocationService } from 'src/app/services/location.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';

import * as mapboxgl from 'mapbox-gl';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.scss'],
  providers:[
    LocationService,
    ToastService,
    AlertService
  ]
})
export class LocationComponent implements OnInit {

  location:any = {};

  mapbox: any = {
    map: {},
    style: 'mapbox://styles/mapbox/streets-v11',
    latitude: 0,
    longitude: 0,
    address: '',
    country: ''
  };

  idLocation: number;

  loadingFindById: boolean;

  constructor(private activatedRoute: ActivatedRoute, private _locationService: LocationService, private _toastService: ToastService, private navController: NavController, private _alertService: AlertService) { }

  ngOnInit() {

    this.idLocation = parseInt(this.activatedRoute.snapshot.paramMap.get('id'));

    this.findById();

  }

  findById(){

    this.loadingFindById = true;

    this._locationService.findById(this.idLocation).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindById = false;
        this.location = res.location;
        this.mapbox.latitude = this.location.latitude;
        this.mapbox.longitude = this.location.longitude;
        this.mapbox.address = this.location.address;
        this.mapbox.country = this.location.country;

        setTimeout(() => {
          
          this.getLocation();

        }, 1000);

      }else if(res.statusCode == 204){

        this.loadingFindById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){
        
        this.loadingFindById = false;
        this._alertService.present();

      }else if(res.statusCode == 404){
        
        this.loadingFindById = false;
        this.location = null;

      }else if(res.statusCode == 500){
        
        this.loadingFindById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindById = false;
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

        let popup = new mapboxgl.Popup({ offset: 25 }).setHTML(`<ul><li>Dirección: ${self.mapbox.address}</li><li>País: ${self.mapbox.country}</li></ul>`);
  
        new mapboxgl.Marker({draggable: false}).setLngLat([self.mapbox.longitude, self.mapbox.latitude]).setPopup(popup).addTo(self.mapbox.map);
    
        self.mapbox.map.resize();
  
      });

    }

  }

}
