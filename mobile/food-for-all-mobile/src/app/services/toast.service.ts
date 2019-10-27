import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastController: ToastController) { }

  
  async present(message: string){

    const toast = await this.toastController.create({
      message: message,
      duration: 3000,
      animated: true,
      closeButtonText:  'OK',
      showCloseButton:  true,
      position: 'bottom',
      cssClass: 'toast'
    });

    toast.present();
  }
}
