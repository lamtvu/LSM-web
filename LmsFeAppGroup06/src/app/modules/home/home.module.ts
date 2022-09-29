import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { MatIconModule } from '@angular/material/icon';
import { HomeCarouselComponent } from './components/home-carousel/home-carousel.component';
import { MatCarouselModule } from '@ngmodule/material-carousel';
import { HomeComponent } from './home.component';
import { LoginComponent } from './components/login/login.component';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [
    HomeComponent,
    HomeCarouselComponent,
    LoginComponent,

  ],
  imports: [
    CommonModule,
    SharedModule,
    HomeRoutingModule,
    MatIconModule,
    MatInputModule,
    MatCardModule,
    MatTabsModule,
    MatCheckboxModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatRadioModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatCarouselModule.forRoot()
  ],
  exports: [
  ]
})
export class HomeModule { }
