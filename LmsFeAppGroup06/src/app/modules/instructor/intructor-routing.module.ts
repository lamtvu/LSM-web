import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { IntrustorContainerComponent } from './components/intrustor-container/intrustor-container.component';
import { CourseListComponent } from './components/CourseManagement/course-list/course-list.component';
import { CourseContainerComponent } from './components/CourseManagement/course-container/course-container.component';
import { CourseCreateComponent } from './components/CourseManagement/course-create/course-create.component';
import { CourseEditComponent } from './components/CourseManagement/course-edit/course-edit.component';
import { EditContentComponent } from './components/CourseManagement/edit-content/edit-content.component';
import { CourseAnalysisComponent } from './components/CourseManagement/course-analysis/course-analysis.component';
import { ChangePasswordComponent } from 'src/app/shared/components/UserInformationManagement/change-password/change-password.component';
import { ChangeInformationComponent } from 'src/app/shared/components/UserInformationManagement/change-information/change-information.component';
import { UserInformationDetailComponent } from 'src/app/shared/components/UserInformationManagement/user-information-detail/user-information-detail.component';
import { UserInformationContainerComponent } from 'src/app/shared/components/UserInformationManagement/user-information-container/user-information-container.component';


const routes: Routes = [
  {
    path: '',
    component: IntrustorContainerComponent,
    children: [
      {
        path:'',
        redirectTo: '/instructor/course-management',
        pathMatch: 'full'
      },
      {
        path: 'course-management',
        component: CourseContainerComponent,
      },
      {
        path: 'course-analysis',
        component: CourseAnalysisComponent,
      },
      {
        path: 'course-edit/:id',
        component: CourseEditComponent,
      },
      {
        path: 'edit-content',
        component: EditContentComponent,
      },
      {
        path: 'course-review',
        component: EditContentComponent,
      },
      {
        path: 'information',
        component: UserInformationContainerComponent,
        children: [
          {
            path:'',
            redirectTo: '/instructor/information/profile',
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
export class IntructorRoutingModule { }

