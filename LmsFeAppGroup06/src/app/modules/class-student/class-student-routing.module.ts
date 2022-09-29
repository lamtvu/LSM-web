import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ClassStudentContainerComponent } from './components/class-student-container/class-student-container.component';
import { StudentClassPlanComponent } from './components/student-class-plan/student-class-plan.component';
import { StudentClassDetailComponent } from './components/student-class-detail/student-class-detail.component';
import { StudentClassNotificationComponent } from './components/student-class-notification/student-class-notification.component';
import { StudentClassExerciseComponent } from './components/student-class-exercise/student-class-exercise.component';
import { StudentClassQuizComponent } from './components/student-class-quiz/student-class-quiz.component';
import { StudentCourseListComponent } from './components/student-course-list/student-course-list.component';
import { StudentManagementComponent } from './components/StudentManagement/student-management/student-management.component';
import { StudentListComponent } from './components/StudentManagement/student-list/student-list.component';
import { StudentScoreListComponent } from './components/StudentManagement/student-score-list/student-score-list.component';
import { ExerciseDetailComponent } from './components/exercise-detail/exercise-detail.component';
import { comment } from 'postcss';
import { QuizDetailComponent } from './components/quiz-detail/quiz-detail.component';
import { NotFoundComponent } from 'src/app/shared/components/not-found/not-found.component';


const routes: Routes = [
  {
    path: '',
    component: ClassStudentContainerComponent,
    children: [
      {
        path: '',
        redirectTo: 'class-detail',
        pathMatch: 'prefix'

      },
      {
        path: 'class-detail',
        component: StudentClassDetailComponent,
        children: [
          {
            path: '',
            redirectTo: 'plan',
            pathMatch: 'full',
          },
          {
            path: 'plan',
            component: StudentClassPlanComponent
          },
          {
            path: 'notification',
            component: StudentClassNotificationComponent
          },
          {
            path: 'exercise',
            children: [
              {
                path: '',
                redirectTo: 'list'
              },
              {
                path: 'list',
                component: StudentClassExerciseComponent
              },
              {
                path: ':id',
                component: ExerciseDetailComponent
              }
            ]
          },
          {
            path: 'quiz',
            children: [
              {
                path: '',
                redirectTo: 'list'
              },
              {
                path: 'list',
                component: StudentClassQuizComponent
              },
              {
                path: 'detail/:id',
                component: QuizDetailComponent
              },
            ]

          }
        ]
      },
      {
        path: 'course-list',
        component: StudentCourseListComponent
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
          }
        ]
      },
      {
        path: 'my-score',
        component: StudentScoreListComponent
      }
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
export class ClassStudentRoutingModule { }
