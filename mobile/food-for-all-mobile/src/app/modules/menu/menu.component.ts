import { Component, OnInit } from '@angular/core';
import { ActionSheetController, NavController } from '@ionic/angular';
import { Facebook } from '@ionic-native/facebook/ngx';
import { UserService } from 'src/app/services/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
  providers:[
    UserService,
    ToastService
  ]
})
export class MenuComponent implements OnInit {

  user:any = {};

  constructor(private actionSheetController: ActionSheetController, private navController: NavController, private facebook: Facebook, private _userService: UserService, private _toastService: ToastService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));


  }


  updateOnLineById(){

    this._userService.updateOnLineById(this.user).subscribe(res => {

      if(res.statusCode == 200){

      }else if(res.statusCode == 204){

      }else if(res.statusCode == 403){

      }else if(res.statusCode == 404){

      }else if(res.statusCode == 500){

        this._toastService.present(res.message);

      }

    }, error => {

      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  logoutFacebook(){

    this.facebook.logout().then(res =>{

    });

  }

  async presentActionSheet(){

    const actionSheet = await this.actionSheetController.create({

      header: 'Configuraciones',
      buttons:[
        {
          text: 'Perfil',
          icon: 'contact',
          handler: () =>{
            this.navController.navigateForward('/profile/' + this.user.id);
          }
        },
        {
          text: 'Cerrar Sesión',
          icon: 'exit',
          handler: () =>{
            this.navController.navigateForward('/');

            this.user.onLine = false;
            this.updateOnLineById();

            if(this.user.isWithFacebook){

              this.logoutFacebook();

            }
            
            localStorage.clear();
          }
        }
      ]

    });

    await actionSheet.present();

  }

}
