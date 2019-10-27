import { Component, OnInit, Input } from '@angular/core';

import { ModalController, NavController } from '@ionic/angular';

import { ProductService } from 'src/app/services/product.service';
import { ToastService } from 'src/app/services/toast.service';
import { StockService } from 'src/app/services/stock.service';
import { environment } from 'src/environments/environment';
import { SelectProductComponent } from '../select-product/select-product.component';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-stock-add',
  templateUrl: './stock-add.component.html',
  styleUrls: ['./stock-add.component.scss'],
  providers:[
    ProductService,
    ToastService,
    StockService,
    AlertService
  ]
})
export class StockAddComponent implements OnInit {

  @Input() idUser: number;

  stock: any = {};
  product: any = {};

  products: Array<any> = new Array<any>();

  loadingFindAll: boolean;
  loadingCreate: boolean = false;

  min: string = new Date().toISOString().split('T')[0];
  max: string = new Date(new Date().setFullYear(new Date().getFullYear() + 2)).toISOString().split('T')[0];

  constructor(private modalController: ModalController, private _productService: ProductService, private _toastService: ToastService, private navController: NavController, private _stockService: StockService, private _alertService: AlertService) { }

  ngOnInit() {

    this.findAll();
    this.stock.idUser = this.idUser;
    this.stock.quantity = 1;

  }

  findAll(){

    this.loadingFindAll = true;

    this._productService.findAll().subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAll = false;
        this.products = res.products;
        this.product = this.products[0];

      }else if(res.statusCode == 204){

        this.loadingFindAll = false;
        this.products = res.products;
        this.product = {};

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

  create(){

    this.loadingCreate = true;
    this.stock.idProduct = this.product.id;
    this.stock.isAvailable = this.stock.quantity == 0 || this.stock.quantity < 0  ? false : this.stock.isAvailable;

    this._stockService.create(this.stock).subscribe(res => {

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

  async selectProduct(){

    const modal = await this.modalController.create({
      component: SelectProductComponent,
    });

    await modal.present();

    const { data } = await modal.onDidDismiss();

    if(data.ok == true){

      this.product = data.product;

    }

  }

}
