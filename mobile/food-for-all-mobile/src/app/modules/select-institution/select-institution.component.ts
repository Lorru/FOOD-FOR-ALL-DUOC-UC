import { Component, OnInit } from '@angular/core';
import { InstitutionService } from 'src/app/services/institution.service';
import { ModalController } from '@ionic/angular';
import { environment } from 'src/environments/environment';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-select-institution',
  templateUrl: './select-institution.component.html',
  styleUrls: ['./select-institution.component.scss'],
  providers:[
    InstitutionService,
    ToastService
  ]
})
export class SelectInstitutionComponent implements OnInit {

  institutions: Array<any> = new Array<any>();

  loadingFindAll: boolean;

  searcher: string = null;

  constructor(private _institutionService: InstitutionService, private modalController: ModalController, private _toastService: ToastService) { }

  ngOnInit() {
    
    this.findAll();

  }

  findAll(){

    this.loadingFindAll = true;

    this._institutionService.findAll(this.searcher).subscribe(res => {

      if(res.statusCode == 200){

        this.loadingFindAll = false;
        this.institutions = res.institutions;

      }else if(res.statusCode == 204){

        this.loadingFindAll = false;
        this.institutions = res.institutions;

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

    this.findAll();

  }

  refresh(e:any){

    this.findAll();
    e.detail.complete();

  }

  selectInstitution(i:number){

    this.modalController.dismiss({ok:true, institution: this.institutions[i]});

  }

  dismiss(){

    this.modalController.dismiss({ok:false});

  }

}
