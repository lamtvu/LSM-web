import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ClassTeacherContainerComponent } from './components/class-teacher-container/class-teacher-container.component';
import { TeacherClassDetailComponent } from './components/teacher-class-detail/teacher-class-detail.component';
import { TeacherCourseManagementComponent } from './components/teacher-course-management/teacher-course-management.component';
import { StudentManagementComponent } from './components/student-management/student-management.component';
import { StudentListComponent } from './components/student-list/student-list.component';
import { RequestListComponent } from './components/request-list/request-list.component';
import { TeacherClassPlanComponent } from './components/teacher-class-plan/teacher-class-plan.component';
import { TeacherClassQuizComponent } from './components/teacher-class-quiz/teacher-class-quiz.component';
import { TeacherClassExerciseComponent } from './components/teacher-class-exercise/teacher-class-exercise.component';
import { TeacherClassNotificationComponent } from './components/teacher-class-notification/teacher-class-notification.component';
import { TeacherScoreListComponent } from './components/teacher-score-list/teacher-score-list.component';
import { TeacherReportListComponent } from './components/teacher-report-list/teacher-report-list.component';

const routes: Routes = [
  {
    path: '',
    component: ClassTeacherContainerComponent,
    children: [
      {
        path: '',
        redirectTo: 'class-detail',
        pathMatch: 'prefix'

      },
      {
        path: 'class-detail',
        component: TeacherClassDetailComponent,
        children: [
          {
            path:'',
            redirectTo:'plan',
            pathMatch:'full',
          },
          {
            path: 'plan',
            component: TeacherClassPlanComponent
          },
          {
            path: 'notification',
            component: TeacherClassNotificationComponent
          },
          {
            path: 'exercise',
            component: TeacherClassExerciseComponent
          },
          {
            path: 'quiz',
            component: TeacherClassQuizComponent

          }
        ]
      },
      {
        path: 'course-management',
        component: TeacherCourseManagementComponent
      },
      {
        path: 'student-management',
        component: StudentManagementComponent,
        children: [
          {
            path: '',
            redirectTo: 'student-list',
            pathMatch: 'full'
          },
          {
            path: 'student-list',
            component: StudentListComponent
          },
          {
            path: 'request-list',
            component: RequestListComponent
          }
        ]
      },
      {
        path: 'score-management',
        component: TeacherScoreListComponent
      },
      {
        path: 'report-list',
        component: TeacherReportListComponent
      },
    ]
  }
]

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ClassTeacherRoutingModule { }
