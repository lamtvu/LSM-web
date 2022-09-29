import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { StudentContainerComponent } from "./student-container/student-container.component";
import { MyClassComponent } from "./components/my-class/my-class.component";
import { SubmisstionQuizComponent } from "./components/submisstion-quiz/submisstion-quiz.component";
import { UserInformationContainerComponent } from "src/app/shared/components/UserInformationManagement/user-information-container/user-information-container.component";
import { UserInformationDetailComponent } from "src/app/shared/components/UserInformationManagement/user-information-detail/user-information-detail.component";
import { ChangeInformationComponent } from "src/app/shared/components/UserInformationManagement/change-information/change-information.component";
import { ChangePasswordComponent } from "src/app/shared/components/UserInformationManagement/change-password/change-password.component";
import { StudentRequestComponent } from "./components/student-request/student-request.component";

const routes: Routes = [
  {
    path: '',
    component: StudentContainerComponent,
    children: [
      {
        path: '',
        redirectTo: '/student/my-class',
        pathMatch: 'full'
      },
      {
        path: 'information',
        component: UserInformationContainerComponent,
        children: [
          {
            path: '',
            redirectTo: '/student/information/profile',
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
      {
        path: 'request',
        component: StudentRequestComponent
      },
      {
        path: 'my-class',
        component: MyClassComponent
      },
      {
        path: 'submission-quiz/:id',
        component: SubmisstionQuizComponent
      },
      {
        path: 'class/:id',
        loadChildren: () => import('./../class-student/class-student.module').then(m => m.ClassStudentModule)
      },
      {
        path: 'course',
        loadChildren: () => import('./../course/course.module').then(m => m.CourseModule)
      }
    ]
  },
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
export class StudentRoutingModule { }
