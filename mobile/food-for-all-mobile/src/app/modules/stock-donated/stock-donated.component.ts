import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SummaryService } from 'src/app/services/summary.service';
import { ToastService } from 'src/app/services/toast.service';
import { Chart } from "chart.js";
import { environment } from 'src/environments/environment';
import { StockReceivedService } from 'src/app/services/stock-received.service';

@Component({
  selector: 'app-stock-donated',
  templateUrl: './stock-donated.component.html',
  styleUrls: ['./stock-donated.component.scss'],
  providers:[
    SummaryService,
    ToastService,
    StockReceivedService
  ]
})
export class StockDonatedComponent implements OnInit {

  @ViewChild("chartDoughnut", { static: false  }) chartDoughnut: ElementRef;

  charts:Array<any> = new Array<any>();

  loadingFindAllChartDoughnut: boolean;

  constructor(private _summaryService: SummaryService, private _toastService: ToastService, private _stockReceivedService: StockReceivedService) { }

  ngOnInit() {

    this._stockReceivedService.startConnectionSignalR();
    this.findAllChartDoughnut();
    this.listenerSignalR();

  }

  findAllChartDoughnut(){

    this.loadingFindAllChartDoughnut = true;

    this._summaryService.findAllChartDoughnut().subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAllChartDoughnut = false;
        this.charts = res.charts;

        this.charts.forEach(chart => {
          
          chart.backgroundColor = this.getBackgroundColorChart(chart.data);
          chart.hoverBackgroundColor = this.getHoverBackgroundColorChart(chart.data);

        });

        setTimeout(() => {
          
          this.getChartDoughnut();

        }, 1000);

      }else if(res.statusCode == 204){

        this.loadingFindAllChartDoughnut = false;
        this.charts = res.charts;

      }else if(res.statusCode == 500){

        this.loadingFindAllChartDoughnut = false;
        this._toastService.present(res.message);

      }

    }, error => {
      console.log(error);
      this.loadingFindAllChartDoughnut = false;
      this._toastService.present(environment.INTERNAL_ERROR_MESSAGE_API);

    });

  }

  getChartDoughnut(){

    this.chartDoughnut = new Chart(this.chartDoughnut.nativeElement, {
      type: "doughnut",
      data: {
        labels: this.charts.map(c => c.label),
        datasets: [
          {
            label: "# de votos",
            data: this.charts.map(c => c.data),
            backgroundColor: this.charts.map(c => c.backgroundColor),
            hoverBackgroundColor: this.charts.map(c => c.hoverBackgroundColor)
          }
        ]
      }
    });


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

  refresh(e:any){

    this.findAllChartDoughnut();
    e.detail.complete();
  }

  listenerSignalR(){

    this._stockReceivedService.hubConnection.on('create', res => {

      this.findAllChartDoughnut();

    })

  }
}
