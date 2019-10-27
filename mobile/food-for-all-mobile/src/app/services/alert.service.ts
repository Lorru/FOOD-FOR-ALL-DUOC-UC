import { Injectable } from '@angular/core';

import { AlertController, NavController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private alertController: AlertController, private navController: NavController) { }

  async present() {
    const alert = await this.alertController.create({
      header: 'Sesión expirada.',
      message: 'Por su seguridad, su sesión esta expirada, presione OK, para volver al Login.',
      buttons: [{
        text: 'OK',
        role: 'cancel',
        handler: () => {
          this.navController.navigateForward('/');
        }
      }]
    });

    await alert.present();
  }
}
