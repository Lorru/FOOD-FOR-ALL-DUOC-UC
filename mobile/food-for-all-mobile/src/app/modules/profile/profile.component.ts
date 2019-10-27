import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ModalController, ActionSheetController, PopoverController } from '@ionic/angular';

import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';
import { CallNumber } from '@ionic-native/call-number/ngx';

import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';
import { UserUpdateComponent } from '../user-update/user-update.component';
import { UserDeleteComponent } from '../user-delete/user-delete.component';
import { CalificationAddComponent } from '../calification-add/calification-add.component';
import { CalificationUserService } from 'src/app/services/calification-user.service';
import { DenouncedAddComponent } from '../denounced-add/denounced-add.component';
import { DenouncedService } from 'src/app/services/denounced.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  providers:[
    UserService,
    ToastService,
    AlertService,
    CalificationUserService,
    DenouncedService
  ]
})
export class ProfileComponent implements OnInit {

  user: any = {};
  calificationUser: any = {};
  denounced: any = {};
  userConnect: any;

  stars: Array<any> = new Array<any>();

  idUser: number;

  loadingFindById: boolean;

  constructor(private activatedRoute: ActivatedRoute, private _userService: UserService, private _toastService: ToastService, private _alertService: AlertService, private camera: Camera, private modalController: ModalController, private photoViewer: PhotoViewer, private actionSheetController: ActionSheetController, private callNumber: CallNumber, private popoverController: PopoverController, private _calificationUserService: CalificationUserService, private _denouncedService: DenouncedService) { }

  ngOnInit() {

    this.idUser = parseInt(this.activatedRoute.snapshot.paramMap.get('id'));
    this.userConnect = JSON.parse(localStorage.getItem('userConnect'));

    this.findById();

  }

  findById(){

    this.loadingFindById = true;

    this._userService.findById(this.idUser).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindById = false;
        this.user = res.user;
        this.stars = new Array(this.user.star == null ? 0 : this.user.star);
        this.findByIdUserAndIdUserCalification();
        this.findByIdUserAndIdUserAccuser();

      }else if(res.statusCode == 204){

        this.loadingFindById = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){
        
        this.loadingFindById = false;
        this._alertService.present();

      }else if(res.statusCode == 404){
        
        this.loadingFindById = false;
        this.user = null;

      }else if(res.statusCode == 500){
        
        this.loadingFindById = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindById = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdUserAndIdUserCalification(){

    this._calificationUserService.findByIdUserAndIdUserCalification(this.idUser, this.userConnect.id).subscribe(res => {

      if(res.statusCode == 200){
        
        this.calificationUser = res.calificationUser;

      }else if(res.statusCode == 204){

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 404){

        this.calificationUser = null;

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }

    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  findByIdUserAndIdUserAccuser(){

    this._denouncedService.findByIdUserAndIdUserAccuser(this.idUser, this.userConnect.id).subscribe(res => {

      if(res.statusCode == 200){
        
        this.denounced = res.denounced;

      }else if(res.statusCode == 204){

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this._alertService.present();

      }else if(res.statusCode == 404){

        this.denounced = null;

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

  getPhotoProfile(){

    if(this.user.photo == null){

      this.photoViewer.show('/assets/img/user.png');

    }else{

      if(this.user.isWithFacebook){

        this.photoViewer.show(this.user.photo);

      }else{

        this.photoViewer.show('data:image/jpeg;base64,' + this.user.photo);

      }

    }

  }

  getCamera(){

    const options: CameraOptions = {
      quality: 60,
      destinationType: this.camera.DestinationType.DATA_URL,
      encodingType: this.camera.EncodingType.JPEG,
      mediaType: this.camera.MediaType.PICTURE,
      correctOrientation: true,
      sourceType: this.camera.PictureSourceType.CAMERA
    }
    
    this.camera.getPicture(options).then(res => {

      this.user.photo = res;
      
      setTimeout(() => {
        
        this.updateById();

      }, 1000);

    });

  }

  getPhotoLibrary(){

    const options: CameraOptions = {
      quality: 60,
      destinationType: this.camera.DestinationType.DATA_URL,
      encodingType: this.camera.EncodingType.JPEG,
      mediaType: this.camera.MediaType.PICTURE,
      correctOrientation: true,
      sourceType: this.camera.PictureSourceType.PHOTOLIBRARY
    }
    
    this.camera.getPicture(options).then(res => {

      this.user.photo = res;
      
      setTimeout(() => {
        
        this.updateById();

      }, 1000);

    });

  }

  updateById(){

    this._userService.updateById(this.user).subscribe(res =>{

      if(res.statusCode == 200){

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

  callDonor(){

    this.callNumber.callNumber(this.user.phone, true).then(res => {


    });
    
  }

  destroyCalification(){

    this._calificationUserService.destroyById(this.calificationUser.id).subscribe(res => {

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

  async addCalification(){

    const modal = await this.popoverController.create({
      component: CalificationAddComponent,
      componentProps:{
        stock: null,
        user: this.user
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
          text: 'Ver Foto',
          icon: 'contact',
          handler: () =>{
            this.getPhotoProfile();
          }
        },
        {
          text: 'Seleccionar Foto',
          icon: 'image',
          handler: () =>{
            this.getPhotoLibrary();
          }
        },
        {
          text: 'Sacar Foto',
          icon: 'camera',
          handler: () =>{
            this.getCamera();
          }
        }
      ]

    });

    await actionSheet.present();

  }

  async presentActionSheetCall(){

    const actionSheet = await this.actionSheetController.create({

      buttons:[
        {
          text: 'Llamada Whatsapp',
          icon: 'logo-whatsapp',
          handler: () =>{
            
            let a = document.createElement('a');
            a.href = 'https://api.whatsapp.com/send?phone=56' + this.user.phone;
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

  async update(){

    const modal = await this.modalController.create({
      component: UserUpdateComponent,
      componentProps:{
        user: this.user
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findById();


  }

  async delete(){

    const modal = await this.modalController.create({
      component: UserDeleteComponent,
      componentProps:{
        user: this.user
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findById();


  }

  async denouncedAdd(){

    let denounced: any = {};

    denounced.idUser = this.user.id;
    denounced.idUserAccuser = this.userConnect.id;

    const modal = await this.modalController.create({
      component: DenouncedAddComponent,
      componentProps:{
        denounced: denounced,
        user: this.user
      }
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    this.findById();


  }
}
