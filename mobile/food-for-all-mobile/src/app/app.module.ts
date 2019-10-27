import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';
import { OneSignal } from '@ionic-native/onesignal/ngx';
import { Geolocation } from '@ionic-native/geolocation/ngx';
import { Facebook } from '@ionic-native/facebook/ngx';
import { Camera } from '@ionic-native/camera/ngx';
import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './modules/login/login.component';
import { MenuModule } from './modules/menu/menu.module';
import { RegisterComponent } from './modules/register/register.component';
import { MainComponent } from './modules/main/main.component';
import { HeaderMainComponent } from './modules/header-main/header-main.component';
import { RecoveryPasswordComponent } from './modules/recovery-password/recovery-password.component';
import { SetPasswordComponent } from './modules/set-password/set-password.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { CallNumber } from '@ionic-native/call-number/ngx';
import { SelectInstitutionComponent } from './modules/select-institution/select-institution.component';
import { StockDonatedComponent } from './modules/stock-donated/stock-donated.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    LoginComponent,
    RegisterComponent,
    HeaderMainComponent,
    RecoveryPasswordComponent,
    SetPasswordComponent,
    SelectInstitutionComponent,
    StockDonatedComponent
  ],
  entryComponents: [
    SelectInstitutionComponent
  ],
  imports: [
    BrowserModule, 
    IonicModule.forRoot(), 
    AppRoutingModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    MenuModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
  ],
  providers: [
    OneSignal,
    Geolocation,
    Facebook,
    Camera,
    PhotoViewer,
    StatusBar,
    SplashScreen,
    CallNumber,
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
