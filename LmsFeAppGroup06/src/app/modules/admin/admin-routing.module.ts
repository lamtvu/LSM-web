import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { ChangeInformationComponent } from '../../shared/components/UserInformationManagement/change-information/change-information.component';
import { ChangePasswordComponent } from '../../shared/components/UserInformationManagement/change-password/change-password.component';
import { UserInformationContainerComponent } from '../../shared/components/UserInformationManagement/user-information-container/user-information-container.component';
import { UserInformationDetailComponent } from '../../shared/components/UserInformationManagement/user-information-detail/user-information-detail.component';
import { AdminContainerComponent } from './admin-container/admin-container.component';
import { AccountContainerComponent } from './components/account-management/account-container/account-container.component';
import { AccountListComponent } from './components/account-management/account-list/account-list.component';
import { CourseContainerComponent } from './components/course-management/course-container/course-container.component';
import { CourseDetailComponent } from './components/course-management/course-detail/course-detail.component';
import { CourseListComponent } from './components/course-management/course-list/course-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminContainerComponent,
    children: [
      {
        path:'',
        redirectTo: '/admin/account-management/account-list',
        pathMatch: 'full'
      },
      {
        path: 'account-management',
        component: AccountContainerComponent,
        children: [
          {
            path:'',
            redirectTo: '/admin/account-management/account-list',
            pathMatch: 'full'
          },
          {
            path: 'account-list',
            component: AccountListComponent
          }
        ]
      },
      {
        path: 'course-management',
        component: CourseContainerComponent,
        children: [
          {
            path:'',
            redirectTo: '/admin/course-management/course-list',
            pathMatch: 'full'
          },
          {
            path: 'course-list',
            component: CourseListComponent
          },
          {
            path: 'course-detail',
            component: CourseDetailComponent
          }
        ]
      },
      {
        path: 'information',
        component: UserInformationContainerComponent,
        children: [
          {
            path: '',
            redirectTo: '/admin/information/profile',
            pathMatch: 'full'
          },
          {
            path: 'profile',
            component: UserInformationDetailComponent
          },
          {
            path: 'change-information',
            component: ChangeInformationComponent
          },
          {
            path: 'change-password',
            component: ChangePasswordComponent
          }
        ]
      },
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule
  ]
})
export class AdminRoutingModule { }

