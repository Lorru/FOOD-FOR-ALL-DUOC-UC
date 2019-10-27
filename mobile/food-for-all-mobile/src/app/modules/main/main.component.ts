import { Component, OnInit } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {

  token: string = null;

  constructor(private navController: NavController) { }

  ngOnInit() {

    // this.token = localStorage.getItem('token');

    // if(this.token == null){

    //   this.navController.navigateForward('/');
    //   localStorage.clear();

    // }else{

    //   this.navController.navigateForward('/menu/home');

    // }

  }

}
