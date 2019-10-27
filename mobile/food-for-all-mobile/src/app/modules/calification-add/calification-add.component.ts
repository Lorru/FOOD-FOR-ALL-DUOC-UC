import { Component, OnInit, Input } from '@angular/core';
import { PopoverController } from '@ionic/angular';

import { CalificationStockService } from 'src/app/services/calification-stock.service';
import { CalificationUserService } from 'src/app/services/calification-user.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-calification-add',
  templateUrl: './calification-add.component.html',
  styleUrls: ['./calification-add.component.scss'],
  providers:[
    CalificationStockService,
    CalificationUserService,
    ToastService,
    AlertService
  ]
})
export class CalificationAddComponent implements OnInit {

  @Input() stock: any;
  @Input() user: any;
  
  starts: Array<any> = [
    { icon: 'star-outline', value: 1},
    { icon: 'star-outline', value: 2},
    { icon: 'star-outline', value: 3},
    { icon: 'star-outline', value: 4},
    { icon: 'star-outline', value: 5}
  ];

  userCalification:any;
  calification: any = {};

  constructor(private popoverController: PopoverController, private _calificationUserService: CalificationUserService, private _calificationStockService: CalificationStockService, private _toastService: ToastService, private _alertService: AlertService) { }

  ngOnInit() {

    this.userCalification = JSON.parse(localStorage.getItem('userConnect'));
    
  }

  selectStar(i: number){

    this.starts.forEach(star => {
      
      star.icon = 'star-outline';

    });

    this.starts[i].icon = 'star';
    this.calification.calification = this.starts[i].value;

  }

  create(){

    if(this.stock != null){

      this.calification.idStock = this.stock.id;
      this.calification.idUserCalification = this.userCalification.id;

      this.createCalificationStock();

    }else if(this.user != null){

      this.calification.idUser = this.user.id;
      this.calification.idUserCalification = this.userCalification.id;

      this.createCalificationUser();

    }

  }

  createCalificationUser(){

    this._calificationUserService.create(this.calification).subscribe(res => {

      if(res.statusCode == 201){

        this._toastService.present(res.message);
        this.popoverController.dismiss({ok:true});

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

  createCalificationStock(){
    
    this._calificationStockService.create(this.calification).subscribe(res => {

      if(res.statusCode == 201){

        this._toastService.present(res.message);
        this.popoverController.dismiss({ok:true});

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

    this.popoverController.dismiss({ok:false});

  }
}
