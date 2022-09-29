import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { MatIconModule } from '@angular/material/icon';
import { FooterComponent } from './components/footer/footer.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RouterModule } from '@angular/router';
import { StarReviewComponent } from './components/star-review/star-review.component';
import { BarChartComponent } from './components/bar-chart/bar-chart.component';
import { ChartsModule } from '@rinminase/ng-charts';
import { PieChartComponent } from './components/pie-chart/pie-chart.component';
import { FormatDataPipe } from './pipes/format-data.pipe';
import { CourseCardComponent } from './components/course-card/course-card.component';
import { MatMenuModule } from '@angular/material/menu';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { UserService } from './services/user.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DeleteDialogComponent } from './components/delete-dialog/delete-dialog.component';
import { NotifyDetailComponent } from './components/notify-detail/notify-detail.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { UserInformationContainerComponent } from './components/UserInformationManagement/user-information-container/user-information-container.component';
import { UserInformationDetailComponent } from './components/UserInformationManagement/user-information-detail/user-information-detail.component';
import { ChangeInformationComponent } from './components/UserInformationManagement/change-information/change-information.component';
import { ChangePasswordComponent } from './components/UserInformationManagement/change-password/change-password.component';



@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    NotFoundComponent,
    StarReviewComponent,
    BarChartComponent,
    DeleteDialogComponent,
    PieChartComponent,
    CourseCardComponent,
    FormatDataPipe,
    SideNavComponent,
    NotifyDetailComponent,
    UserInformationContainerComponent,
    UserInformationDetailComponent,
    ChangeInformationComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    ChartsModule,
    MatDialogModule,
    HttpClientModule,
    MatMenuModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatRadioModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    RouterModule
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    BarChartComponent,
    StarReviewComponent,
    PieChartComponent,
    CourseCardComponent,
    FormatDataPipe,
    SideNavComponent,
    UserInformationContainerComponent,
    UserInformationDetailComponent,
    ChangeInformationComponent,
    ChangePasswordComponent
  ],
  providers:[
    UserService
  ]
})
export class SharedModule { }
