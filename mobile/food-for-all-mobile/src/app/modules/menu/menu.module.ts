import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { MenuRoutingModule } from './menu-routing.module';
import { MenuComponent } from './menu.component';
import { HomeComponent } from '../home/home.component';
import { ProfileComponent } from '../profile/profile.component';
import { StocksComponent } from '../stocks/stocks.component';
import { StockComponent } from '../stock/stock.component';
import { StockAddComponent } from '../stock-add/stock-add.component';
import { StockUpdateComponent } from '../stock-update/stock-update.component';
import { SelectProductComponent } from '../select-product/select-product.component';
import { StockObservationComponent } from '../stock-observation/stock-observation.component';
import { StockCommentAddComponent } from '../stock-comment-add/stock-comment-add.component';
import { StockSearcherComponent } from '../stock-searcher/stock-searcher.component';
import { StockLocationComponent } from '../stock-location/stock-location.component';
import { LocationsComponent } from '../locations/locations.component';
import { LocationAddComponent } from '../location-add/location-add.component';
import { LocationComponent } from '../location/location.component';
import { StockReceivedAddComponent } from '../stock-received-add/stock-received-add.component';
import { StockAvailableAddComponent } from '../stock-available-add/stock-available-add.component';
import { StockAvailableDestroyComponent } from '../stock-available-destroy/stock-available-destroy.component';
import { ProductsBeneficiaryComponent } from '../products-beneficiary/products-beneficiary.component';
import { UserUpdateComponent } from '../user-update/user-update.component';
import { UserDeleteComponent } from '../user-delete/user-delete.component';
import { StockCommentDeleteComponent } from '../stock-comment-delete/stock-comment-delete.component';
import { StockAvailableDonorComponent } from '../stock-available-donor/stock-available-donor.component';
import { CalificationAddComponent } from '../calification-add/calification-add.component';
import { MessageComponent } from '../message/message.component';
import { MessagesComponent } from '../messages/messages.component';
import { SelectUserMessageComponent } from '../select-user-message/select-user-message.component';
import { DenouncedAddComponent } from '../denounced-add/denounced-add.component';
import { StockImageComponent } from '../stock-image/stock-image.component';

@NgModule({
  declarations: [
    MenuComponent,
    HomeComponent,
    ProfileComponent,
    StocksComponent,
    StockComponent,
    StockAddComponent,
    StockUpdateComponent,
    SelectProductComponent,
    StockObservationComponent,
    StockCommentAddComponent,
    StockSearcherComponent,
    StockLocationComponent,
    LocationsComponent,
    LocationAddComponent,
    LocationComponent,
    StockReceivedAddComponent,
    StockAvailableAddComponent,
    StockAvailableDestroyComponent,
    ProductsBeneficiaryComponent,
    UserUpdateComponent,
    UserDeleteComponent,
    StockCommentDeleteComponent,
    StockAvailableDonorComponent,
    CalificationAddComponent,
    MessageComponent,
    MessagesComponent,
    SelectUserMessageComponent,
    DenouncedAddComponent,
    StockImageComponent
  ],
  exports:[
    MenuComponent,
    HomeComponent,
    ProfileComponent,
    StocksComponent,
    StockComponent,
    StockAddComponent,
    StockUpdateComponent,
    SelectProductComponent,
    StockObservationComponent,
    StockCommentAddComponent,
    StockSearcherComponent,
    StockLocationComponent,
    LocationsComponent,
    LocationAddComponent,
    LocationComponent,
    StockReceivedAddComponent,
    StockAvailableAddComponent,
    StockAvailableDestroyComponent,
    ProductsBeneficiaryComponent,
    UserUpdateComponent,
    UserDeleteComponent,
    StockCommentDeleteComponent,
    StockAvailableDonorComponent,
    CalificationAddComponent,
    MessageComponent,
    MessagesComponent,
    SelectUserMessageComponent,
    DenouncedAddComponent,
    StockImageComponent
  ],
  imports: [
    CommonModule,
    MenuRoutingModule,
    IonicModule,
    FormsModule,
  ],
  entryComponents:[
    StockAddComponent,
    StockUpdateComponent,
    SelectProductComponent,
    StockObservationComponent,
    StockCommentAddComponent,
    StockLocationComponent,
    LocationAddComponent,
    StockReceivedAddComponent,
    StockAvailableAddComponent,
    StockAvailableDestroyComponent,
    UserUpdateComponent,
    UserDeleteComponent,
    StockCommentDeleteComponent,
    CalificationAddComponent,
    SelectUserMessageComponent,
    DenouncedAddComponent,
    StockImageComponent
  ]
})
export class MenuModule { }
