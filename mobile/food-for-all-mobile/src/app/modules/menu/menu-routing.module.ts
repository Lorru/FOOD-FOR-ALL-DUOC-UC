import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MenuComponent } from './menu.component';
import { HomeComponent } from '../home/home.component';
import { ProfileComponent } from '../profile/profile.component';
import { StocksComponent } from '../stocks/stocks.component';
import { StockComponent } from '../stock/stock.component';
import { StockSearcherComponent } from '../stock-searcher/stock-searcher.component';
import { LocationsComponent } from '../locations/locations.component';
import { LocationComponent } from '../location/location.component';
import { ProductsBeneficiaryComponent } from '../products-beneficiary/products-beneficiary.component';
import { StockAvailableDonorComponent } from '../stock-available-donor/stock-available-donor.component';
import { MessagesComponent } from '../messages/messages.component';
import { MessageComponent } from '../message/message.component';


const routes: Routes = [
  { path: 'menu', component:  MenuComponent,  children: [
     {  path: 'home', component:  HomeComponent },
     {  path: 'stocks', component:  StocksComponent },
     {  path: 'stock-searcher', component:  StockSearcherComponent },
     {  path: 'locations', component:  LocationsComponent },
     {  path: 'products-beneficiary', component:  ProductsBeneficiaryComponent },
     {  path: 'messages', component:  MessagesComponent },
  ]},
  {  path: 'profile/:id', component:  ProfileComponent },
  {  path: 'stock/:id', component:  StockComponent },
  {  path: 'location/:id', component:  LocationComponent },
  {  path: 'stock-available-donor/:id', component:  StockAvailableDonorComponent },
  {  path: 'message/:idUserSend/:idUserReceived', component:  MessageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MenuRoutingModule { }
