import { Component, OnInit, Input } from '@angular/core';
import { StockImageService } from 'src/app/services/stock-image.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { ModalController } from '@ionic/angular';

import { Camera, CameraOptions } from '@ionic-native/camera/ngx';
import { environment } from 'src/environments/environment';
import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';

@Component({
  selector: 'app-stock-image',
  templateUrl: './stock-image.component.html',
  styleUrls: ['./stock-image.component.scss'],
  providers:[
    StockImageService,
    ToastService,
    AlertService
  ]
})
export class StockImageComponent implements OnInit {

  @Input() stock: any;

  stockImage: any = {};

  stockImages: Array<any> = new Array<any>();
  selectionImages: Array<any> = [
    { icon: 'camera', color: 'dark'  },
    { icon: 'image',  color: 'dark'  }
  ];

  selectionImage: string = 'create';

  loadingFindByIdStock: boolean;

  constructor(private _stockImageService: StockImageService, private _toastService: ToastService, private _alertService: AlertService, private modalController: ModalController, private camera: Camera, private photoViewer: PhotoViewer) { }

  ngOnInit() {

    this.findByIdStock();

  }

  selectOptionImage(i:number){

    this.selectionImages.forEach(selectionImage => {
      
      selectionImage.color = 'dark';

    });

    this.selectionImage = this.selectionImages[i].icon;
    this.selectionImages[i].color = 'primary';

    if(this.selectionImage == 'camera'){

      this.getCamera();

    }else if(this.selectionImage == 'image'){

      this.getPhotoLibrary();

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

      this.stockImage.referenceImage = res;
      this.stockImage.idStock = this.stock.id;

      this.create();

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

      this.stockImage.referenceImage = res;
      this.stockImage.idStock = this.stock.id;

      this.create();

    });

  }

  findByIdStock(){

    this.loadingFindByIdStock = true;

    this._stockImageService.findByIdStock(this.stock.id).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindByIdStock = false;
        this.stockImages = res.stockImages;

      }else if(res.statusCode == 204){

        this.loadingFindByIdStock = false;
        this.stockImages = res.stockImages;

        if(res.message){

          this._toastService.present(res.message);

        }

      }else if(res.statusCode == 403){

        this.loadingFindByIdStock = false;
        this.modalController.dismiss({ok:false});
        this._alertService.present();

      }else if(res.statusCode == 500){

        this.loadingFindByIdStock = false;
        this._toastService.present(res.message);

      }


    }, error => {

      this.loadingFindByIdStock = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  create(){

    let stockImage: any = JSON.parse(JSON.stringify(this.stockImage));

    this._stockImageService.create(stockImage).subscribe(res => {

      if(res.statusCode == 201){

        this.stockImages.push(stockImage);
        this._toastService.present(res.message);

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

  getPhotoStock(i: number){

    this.photoViewer.show('data:image/jpeg;base64,' + this.stockImages[i].referenceImage);

  }

  destroyById(i: number){

    let stockImage: any = this.stockImages[i];

    this._stockImageService.destroyById(stockImage.id).subscribe(res => {

      if(res.statusCode == 200){

        this.stockImages = this.stockImages.filter(si => si.id != stockImage.id);
        this._toastService.present(res.message);

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

  dismiss(){

    this.modalController.dismiss({ok:false});

  }
}
