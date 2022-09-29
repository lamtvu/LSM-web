import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ChangeInformationComponent } from "src/app/shared/components/UserInformationManagement/change-information/change-information.component";
import { ChangePasswordComponent } from "src/app/shared/components/UserInformationManagement/change-password/change-password.component";
import { UserInformationContainerComponent } from "src/app/shared/components/UserInformationManagement/user-information-container/user-information-container.component";
import { UserInformationDetailComponent } from "src/app/shared/components/UserInformationManagement/user-information-detail/user-information-detail.component";
import { ClassManagementComponent } from "./components/ClassManagement/class-management/class-management.component";
import { QuestionManagementComponent } from "./components/question-management/question-management.component";
import { TeacherContainerComponent } from "./components/teacher-container/teacher-container.component";

const routes: Routes = [
  {
    path: '',
    component: TeacherContainerComponent,
    children: [
      {
        path: '',
        redirectTo: '/teacher/class-management',
        pathMatch: 'full'
      },
      {
        path: 'class-management',
        component: ClassManagementComponent,
      },
      {
        path:'class/:id',
        loadChildren: ()=>import('./../class-teacher/class-teacher.module').then(m=>m.ClassTeacherModule)
      },
      {
        path:'quiz/:id',
        component: QuestionManagementComponent
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
      }
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class TeacherRoutingModule { }
