import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';

import { PhotoViewer } from '@ionic-native/photo-viewer/ngx';

import { SummaryService } from 'src/app/services/summary.service';
import { ToastService } from 'src/app/services/toast.service';
import { AlertService } from 'src/app/services/alert.service';

import { Chart } from "chart.js";
import { environment } from 'src/environments/environment';
import { StockReceivedService } from 'src/app/services/stock-received.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers:[
    SummaryService,
    ToastService,
    AlertService,
    StockReceivedService,
    UserService
  ]
})
export class HomeComponent implements OnInit {

  @ViewChild("chartBar", { static: false  }) chartBar: ElementRef;
  @ViewChild("chartDoughnut", { static: false  }) chartDoughnut: ElementRef;
  @ViewChild("chartDoughnutDonor", { static: false  }) chartDoughnutDonor: ElementRef;

  user: any;
  userDonorMaxStockRetirated: any = {};
  stockCommentLastBeneficiary: any = {};
  stockCommentLastDonor: any = {};
  productLessReceived: any = {};
  productMoreReceived: any = {};
  productLastReceived: any = {};
  productLastStock: any = {};
  productLessQuantityStock: any = {};

  chartsDoughnut: Array<any> = new Array<any>();
  chartsDoughnutDonor: Array<any> = new Array<any>();
  chartsBar: Array<any> = new Array<any>();
  stars: Array<any> = new Array<any>();

  loadingFindByIdUser: boolean;

  constructor(private _summaryService: SummaryService, private _toastService: ToastService, private _alertService: AlertService, private photoViewer: PhotoViewer, private _stockReceivedService: StockReceivedService, private _userService: UserService) { }

  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem('userConnect'));

    this.findByIdUser();
    this.updateOnLineById();
    this._stockReceivedService.startConnectionSignalR();
    this.listenerSignalR();
  }

  getPhotoProfile(){

    if(this.user.photo == null){

      this.photoViewer.show('/assets/img/user.png');

    }else{

      if(this.user.isWithFacebook){

        this.photoViewer.show(this.user.photo);

      }else{

        this.photoViewer.show('data:image/jpeg;base64,' + this.user.photo);

      }

    }

  }

  getPhotoCommentLastDonor(){

    this.photoViewer.show('data:image/jpeg;base64,' + this.stockCommentLastDonor.comment);

  }

  getPhotoCommentLastBeneficiary(){

    this.photoViewer.show('data:image/jpeg;base64,' + this.stockCommentLastBeneficiary.comment);

  }

  findByIdUser(){

    this.loadingFindByIdUser = true;

    this._summaryService.findByIdUser(this.user.id).subscribe(res => {


      if(res.statusCode == 200){

        this.loadingFindByIdUser = false;
        this.user = res.user;
        this.stars = new Array(this.user.star == null ? 0 : this.user.star);
        this.userDonorMaxStockRetirated = res.userDonorMaxStockRetirated;
        this.stockCommentLastBeneficiary = res.stockCommentLastBeneficiary;
        this.stockCommentLastDonor = res.stockCommentLastDonor;
        this.productLessReceived = res.productLessReceived;
        this.productMoreReceived = res.productMoreReceived;
        this.productLastReceived = res.productLastReceived;
        this.productLastStock = res.productLastStock;
        this.productLessQuantityStock = res.productLessQuantityStock;
        this.chartsDoughnut = res.chartsDoughnut;
        this.chartsDoughnutDonor = res.chartsDoughnutDonor;
        this.chartsBar = res.chartsBar;

        this.chartsBar.forEach(chart => {
          
          chart.backgroundColor = this.getBackgroundColorChart(chart.data);
          chart.borderColor = this.getBorderColorChart(chart.data);

        });

        this.chartsDoughnut.forEach(chart => {
          
          chart.backgroundColor = this.getBackgroundColorChart(chart.data);
          chart.hoverBackgroundColor = this.getHoverBackgroundColorChart(chart.data);

        });

        this.chartsDoughnutDonor.forEach(chart => {
          
          chart.backgroundColor = this.getBackgroundColorChart(chart.data);
          chart.hoverBackgroundColor = this.getHoverBackgroundColorChart(chart.data);

        });

        setTimeout(() => {
      
          this.getChartBar();

          if(this.chartsDoughnut.length > 0){

            this.getChartDoughnut();

          }

          if(this.chartsDoughnutDonor.length > 0){

            this.getChartDoughnutDonor();

          }


    
        }, 1000);

      }else if(res.statusCode == 204){

        this.loadingFindByIdUser = false;
        this._toastService.present(res.message);

      }else if(res.statusCode == 403){
        
        this.loadingFindByIdUser = false;
        this._alertService.present();

      }else if(res.statusCode == 404){
        
        this.loadingFindByIdUser = false;
        this.user = null;
        this.userDonorMaxStockRetirated = null;
        this.stockCommentLastBeneficiary = null;
        this.stockCommentLastDonor = null;
        this.productLessReceived = null;
        this.productMoreReceived = null;
        this.productLastReceived = null;
        this.productLastStock = null;
        this.productLessQuantityStock = null;
        this.chartsDoughnut = [];
        this.chartsDoughnutDonor = [];
        this.chartsBar = [];

      }else if(res.statusCode == 500){
        
        this.loadingFindByIdUser = false;
        this._toastService.present(res.message);

      }

    }, error => {

      this.loadingFindByIdUser = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  refresh(e:any){

    this.findByIdUser();
    e.detail.complete();

  }

  getChartBar(){

    this.chartBar = new Chart(this.chartBar.nativeElement, {
      type: 'bar',
      data:{
        labels: this.chartsBar.map(c => c.label),
        datasets: [{
          label: "# de votos",
          data: this.chartsBar.map(c => c.data),
          backgroundColor: [
            this.chartsBar.map(c => c.backgroundColor)
          ],
          borderColor: [
            this.chartsBar.map(c => c.borderColor)
          ],
          borderWidth: 1
        }]
      },
      options:{
        scales:{
          yAxes:[
            {
              ticks:{
                beginAtZero: true
              }
            }
          ]
        }
      }
    });

  }

  getChartDoughnut(){

    this.chartDoughnut = new Chart(this.chartDoughnut.nativeElement, {
      type: "doughnut",
      data: {
        labels: this.chartsDoughnut.map(c => c.label),
        datasets: [
          {
            label: "# de votos",
            data: this.chartsDoughnut.map(c => c.data),
            backgroundColor: this.chartsDoughnut.map(c => c.backgroundColor),
            hoverBackgroundColor: this.chartsDoughnut.map(c => c.hoverBackgroundColor)
          }
        ]
      }
    });


  }

  getChartDoughnutDonor(){

    this.chartDoughnutDonor = new Chart(this.chartDoughnutDonor.nativeElement, {
      type: "doughnut",
      data: {
        labels: this.chartsDoughnutDonor.map(c => c.label),
        datasets: [
          {
            label: "# de votos",
            data: this.chartsDoughnutDonor.map(c => c.data),
            backgroundColor: this.chartsDoughnutDonor.map(c => c.backgroundColor),
            hoverBackgroundColor: this.chartsDoughnutDonor.map(c => c.hoverBackgroundColor)
          }
        ]
      }
    });

  }

  getBackgroundColorChart(data: number): string{

    let color: string;

    if(data < 5 && data >= 1){

      color = '#66FFFF';

    }else if(data < 10 && data >= 5){

      color = '#33FF66';

    }else if(data < 15 && data >= 10){

      color = '#33FF66';

    }else if(data < 20 && data >= 15){

      color = '#FFCC66';

    }else if(data < 25 && data >= 20){

      color = '#FF6666';

    }else if(data < 30 && data >= 25){

      color = '#CC0066';

    }else{

      color = '#FF0000';

    }

    return color;

  }

  getHoverBackgroundColorChart(data: number): string{

    let color: string;

    if(data < 5 && data >= 1){

      color = '#66FFFF';

    }else if(data < 10 && data >= 5){

      color = '#33FF66';

    }else if(data < 15 && data >= 10){

      color = '#33FF66';

    }else if(data < 20 && data >= 15){

      color = '#FFCC66';

    }else if(data < 25 && data >= 20){

      color = '#FF6666';

    }else if(data < 30 && data >= 25){

      color = '#CC0066';

    }else{

      color = '#FF0000';

    }

    return color;

  }

  getBorderColorChart(data: number): string{

    let color: string;

    if(data < 5 && data >= 1){

      color = '#66FFFF';

    }else if(data < 10 && data >= 5){

      color = '#33FF66';

    }else if(data < 15 && data >= 10){

      color = '#33FF66';

    }else if(data < 20 && data >= 15){

      color = '#FFCC66';

    }else if(data < 25 && data >= 20){

      color = '#FF6666';

    }else if(data < 30 && data >= 25){

      color = '#CC0066';

    }else{

      color = '#FF0000';

    }

    return color;

  }

  listenerSignalR(){

    this._stockReceivedService.hubConnection.on('create', res => {

      if(res.idStockNavigation.idUser == this.user.id){

        this.ngOnInit();

      }

    })

  }

  updateOnLineById(){

    this.user.onLine = true;
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


}
