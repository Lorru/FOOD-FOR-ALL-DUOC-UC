import { Component, OnInit } from '@angular/core';

import { NavController, ModalController } from '@ionic/angular';

import { LocationService } from 'src/app/services/location.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { LocationAddComponent } from '../location-add/location-add.component';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.scss'],
  providers:[
    LocationService,
    ToastService,
    AlertService
  ]
})
export class LocationsComponent implements OnInit {

  user:any = {};

  locations: Array<any> = new Array<any>();

  loadingFindByIdUser: boolean;

  canMain: boolean;

  constructor(private _locationService: LocationService, private _toastService: ToastService, private navController: NavController, private modalController: ModalController, private _alertService: AlertService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));
    this.findByIdUser();

  }

  findByIdUser(){

    this.loadingFindByIdUser = true;

    this._locationService.findByIdUser(this.user.id).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdUser = false;
        this.locations = res.locations;

        this.canMain = this.locations.filter(l => l.isMain).length == 0 ? true : false;


      }else if(res.statusCode == 204){

        this.loadingFindByIdUser = false;
        this.locations = res.locations;

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

  refresh(e: any){

    this.findByIdUser();
    e.detail.complete();

  }

  updateIsMainById(i: number, value: boolean){

    let location = JSON.parse(JSON.stringify(this.locations[i]));
    location.isMain = value;

    this._locationService.updateIsMainById(location).subscribe(res => {

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

  deleteById(i: number){

    this._locationService.deleteById(this.locations[i].id).subscribe(res => {

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

  async add(){

    const modal = await this.modalController.create({
      component: LocationAddComponent,
      componentProps:{
        idUser: this.user.id,
        locations: this.locations
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.ngOnInit();

  }

}
