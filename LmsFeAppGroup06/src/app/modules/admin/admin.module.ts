import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminContainerComponent } from './admin-container/admin-container.component';
import { RouterModule } from '@angular/router';
import { AccountContainerComponent } from './components/account-management/account-container/account-container.component';
import { AccountListComponent } from './components/account-management/account-list/account-list.component';
import { AdminRoutingModule } from './admin-routing.module';
import {MatIconModule} from '@angular/material/icon';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountDetailComponent } from './components/account-management/account-detail/account-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CourseContainerComponent } from './components/course-management/course-container/course-container.component';
import { CourseDetailComponent } from './components/course-management/course-detail/course-detail.component';
import { CourseListComponent } from './components/course-management/course-list/course-list.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ChangeInformationComponent } from './components/information-management/change-information/change-information.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { LockDialogComponent } from './components/lock-dialog/lock-dialog.component';
import { AdminService } from './services/admin.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { UserInformationContainerComponent } from './components/information-management/user-information-container/user-information-container.component';
import { UserInformationDetailComponent } from './components/information-management/user-information-detail/user-information-detail.component';
import { ChangePasswordComponent } from './components/information-management/change-password/change-password.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';


@NgModule({
  declarations: [
    AdminContainerComponent,
    AccountContainerComponent,
    AccountListComponent,
    AccountDetailComponent,
    CourseContainerComponent,
    CourseDetailComponent,
    CourseListComponent,
    ChangeInformationComponent,
    LockDialogComponent,
    UserInformationContainerComponent,
    UserInformationDetailComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    AdminRoutingModule,
    MatIconModule,
    FormsModule,
    SharedModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatProgressBarModule,
    MatTabsModule,
    MatCardModule,
    HttpClientModule
  ],
  providers:[
    AdminService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class AdminModule { }
