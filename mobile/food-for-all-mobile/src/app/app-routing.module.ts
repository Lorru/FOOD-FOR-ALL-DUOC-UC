import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './modules/login/login.component';
import { RegisterComponent } from './modules/register/register.component';
import { MainComponent } from './modules/main/main.component';
import { RecoveryPasswordComponent } from './modules/recovery-password/recovery-password.component';
import { SetPasswordComponent } from './modules/set-password/set-password.component';
import { StockDonatedComponent } from './modules/stock-donated/stock-donated.component';

const routes: Routes = [
  { path: '', component:  MainComponent  },
  { path: 'login', component:  LoginComponent  },
  { path: 'register', component:  RegisterComponent  },
  { path: 'recovery-password', component:  RecoveryPasswordComponent  },
  { path: 'set-password/:token', component:  SetPasswordComponent  },
  { path: 'stock-donated', component:  StockDonatedComponent  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
