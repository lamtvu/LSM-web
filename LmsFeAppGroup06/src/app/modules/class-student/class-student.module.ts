import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassStudentContainerComponent } from './components/class-student-container/class-student-container.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ClassStudentRoutingModule } from './class-student-routing.module';
import { MatTabsModule } from '@angular/material/tabs';
import { StudentClassPlanComponent } from './components/student-class-plan/student-class-plan.component';
import { StudentClassDetailComponent } from './components/student-class-detail/student-class-detail.component';
import { StudentClassNotificationComponent } from './components/student-class-notification/student-class-notification.component';
import { StudentClassExerciseComponent } from './components/student-class-exercise/student-class-exercise.component';
import { StudentClassQuizComponent } from './components/student-class-quiz/student-class-quiz.component';
import { StudentCourseListComponent } from './components/student-course-list/student-course-list.component';
import { StudentManagementComponent } from './components/StudentManagement/student-management/student-management.component';
import { StudentListComponent } from './components/StudentManagement/student-list/student-list.component';
import { StudentScoreListComponent } from './components/StudentManagement/student-score-list/student-score-list.component';
import { ClassService } from './services/class.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { PlanService } from './services/plan.service';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ExerciseService } from './services/exercise.service';
import { ExerciseDetailComponent } from './components/exercise-detail/exercise-detail.component';
import { SubmisstionExerciseService } from './services/submisstion-exercise.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { QuizService } from './services/quiz.service';
import { QuizDetailComponent } from './components/quiz-detail/quiz-detail.component';
import { SubmisstionQuizService } from './services/submisstion-quiz.service';
import { MatInputModule } from '@angular/material/input';
import { SendReportDialogComponent } from './components/send-report-dialog/send-report-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReportService } from '../class-teacher/services/report.service';
import { ClassCourseService } from './services/class-course.service';



@NgModule({
  declarations: [
    ClassStudentContainerComponent,
    StudentClassPlanComponent,
    StudentClassDetailComponent,
    StudentClassNotificationComponent,
    StudentClassExerciseComponent,
    StudentClassQuizComponent,
    StudentCourseListComponent,
    StudentManagementComponent,
    StudentListComponent,
    StudentScoreListComponent,
    ExerciseDetailComponent,
    QuizDetailComponent,
    SendReportDialogComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    SharedModule,
    MatSelectModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatProgressBarModule,
    ClassStudentRoutingModule,
    MatDialogModule,
    MatTabsModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers:[
    ClassService,
    PlanService,
    ExerciseService,
    SubmisstionExerciseService,
    ClassCourseService,
    SubmisstionQuizService,
    QuizService,
    ReportService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class ClassStudentModule { }
