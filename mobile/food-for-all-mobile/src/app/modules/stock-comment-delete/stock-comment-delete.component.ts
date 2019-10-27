import { Component, OnInit, Input } from '@angular/core';

import { PopoverController } from '@ionic/angular';

import { StockCommentService } from 'src/app/services/stock-comment.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-stock-comment-delete',
  templateUrl: './stock-comment-delete.component.html',
  styleUrls: ['./stock-comment-delete.component.scss'],
  providers:[
    StockCommentService,
    ToastService,
    AlertService
  ]
})
export class StockCommentDeleteComponent implements OnInit {

  @Input() stockComment: any;

  loadingDeleteById: boolean;

  constructor(private _stockCommentService: StockCommentService, private _toastService: ToastService, private _alertService: AlertService, private popoverController: PopoverController) { }

  ngOnInit() {}

  destroyById(){

    this._stockCommentService.destroyById(this.stockComment.id).subscribe(res => {

      if(res.statusCode == 200){

        this.popoverController.dismiss({ok:true});
        this._toastService.present(res.message);

      }else if(res.statusCode == 204){

        this._toastService.present(res.message);

      }else if(res.statusCode == 403){

        this._alertService.present();
        this.popoverController.dismiss({ok:false});

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
