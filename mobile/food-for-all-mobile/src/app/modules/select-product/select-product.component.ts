import { Component, OnInit } from '@angular/core';

import { NavController, ModalController } from '@ionic/angular';

import { ProductService } from 'src/app/services/product.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-select-product',
  templateUrl: './select-product.component.html',
  styleUrls: ['./select-product.component.scss'],
  providers:[
    ProductService,
    ToastService,
    AlertService
  ]
})
export class SelectProductComponent implements OnInit {

  productTypes: Array<any> = new Array<any>();
  products: Array<any> = new Array<any>();

  loadingFindAll: boolean;

  searcher: string = null;

  constructor(private modalController: ModalController, private _productService: ProductService, private _toastService: ToastService, private navController: NavController, private _alertService: AlertService) { }

  ngOnInit() {
    
    this.findAll();

  }

  findAll(){

    this.loadingFindAll = true;

    this._productService.findAll(this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAll = false;
        this.products = res.products;

        this.products.forEach(product => {
          
          if(this.productTypes.find(pt => pt.id == product.idProductType) == null){

            this.productTypes.push(product.idProductTypeNavigation);

          }

        });

      }else if(res.statusCode == 204){

        this.loadingFindAll = false;
        this.products = res.products;
        this.productTypes = [];

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

  search(){

    this.productTypes = [];
    this.findAll();

  }

  refresh(e:any){

    this.productTypes = [];
    this.findAll();
    e.detail.complete();

  }

  selectProduct(i:number){

    this.modalController.dismiss({ok:true, product: this.products[i]});

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }
}
