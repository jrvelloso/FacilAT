import { Injectable } from '@angular/core';
import { Routes } from '@angular/router';
import { DeclaracoesIvaComponent } from 'src/app/declaracoes-iva/declaracoes-iva.component';
import { HomeComponent } from 'src/app/home/home.component';
import { ListUserComponent } from 'src/app/list-user/list-user.component';



@Injectable({
  providedIn: 'root'
})

export class RouterService {

  constructor() { }

  static getRoutes(): Routes {
    return [
      { path: '', component: HomeComponent }, //canActivate: [AuthGuard] },
      { path: 'home', component: HomeComponent }, //canActivate: [AuthGuard] },
      { path: 'user', component: ListUserComponent }, //canActivate: [AuthGuard] },
      { path: 'declaracoes-iva/:userId', component: DeclaracoesIvaComponent }
    ];
  }
}
