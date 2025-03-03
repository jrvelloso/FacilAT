import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { NgTemplateOutlet } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeclaracoesIvaComponent } from './declaracoes-iva/declaracoes-iva.component';
import { HomeComponent } from './home/home.component';
import { ListUserComponent } from './list-user/list-user.component';
import { RecibosVerdesComponent } from './recibos-verdes/recibos-verdes.component';
import { FooterComponent } from './static/footer/footer.component';
import { HeaderComponent } from './static/header/header.component';
import { LoaderComponent } from './static/loader/loader.component';
import { NavBarComponent } from './static/nav-bar/nav-bar.component';
import { SideBarComponent } from './static/side-bar/side-bar.component';
@NgModule({
  declarations: [
    AppComponent,
    AppComponent,
    HomeComponent,
    NavBarComponent,
    HeaderComponent,
    FooterComponent,
    LoaderComponent,
    SideBarComponent,
    RecibosVerdesComponent,
    DeclaracoesIvaComponent,
    DashboardComponent,
    ListUserComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    NgTemplateOutlet,
    NgChartsModule,
    HttpClientJsonpModule,
  ],

  providers: [
    // { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    // { provide: LOCALE_ID, useValue: 'pt-BR' },
    // { provide: APP_BASE_HREF, useValue: '/' },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
