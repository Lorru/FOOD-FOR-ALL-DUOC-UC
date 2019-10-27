import { Component, OnInit, Input } from '@angular/core';
import { DenouncedService } from 'src/app/services/denounced.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { ModalController } from '@ionic/angular';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-denounced-add',
  templateUrl: './denounced-add.component.html',
  styleUrls: ['./denounced-add.component.scss'],
  providers:[
    DenouncedService,
    ToastService,
    AlertService
  ]
})
export class DenouncedAddComponent implements OnInit {

  @Input() denounced: any;
  @Input() user: any;

  loadingCreate: boolean;

  constructor(private _denouncedService: DenouncedService, private _alertService: AlertService, private _toastService: ToastService, private modalController: ModalController) { }

  ngOnInit() {

  }

  create(){

    this.loadingCreate = true;

    this._denouncedService.create(this.denounced).subscribe(res => {

      if(res.statusCode == 201){

        this.loadingCreate = false;
        this._toastService.present(res.message);
        this.modalController.dismiss({ok:true});

      }else if(res.statusCode == 204){

        this.loadingCreate = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this.loadingCreate = false;
        this._alertService.present();

      }else if(res.statusCode == 404){

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
