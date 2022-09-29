import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassTeacherContainerComponent } from './components/class-teacher-container/class-teacher-container.component';
import { TeacherCourseManagementComponent } from './components/teacher-course-management/teacher-course-management.component';
import { StudentManagementComponent } from './components/student-management/student-management.component';
import { TeacherClassDetailComponent } from './components/teacher-class-detail/teacher-class-detail.component';
import { TeacherClassPlanComponent } from './components/teacher-class-plan/teacher-class-plan.component';
import { TeacherClassExerciseComponent } from './components/teacher-class-exercise/teacher-class-exercise.component';
import { TeacherClassQuizComponent } from './components/teacher-class-quiz/teacher-class-quiz.component';
import { TeacherClassNotificationComponent } from './components/teacher-class-notification/teacher-class-notification.component';
import { ClassTeacherRoutingModule } from './class-teacher-routing.module';
import { MatTabsModule } from '@angular/material/tabs';
import { StudentListComponent } from './components/student-list/student-list.component';
import { RequestListComponent } from './components/request-list/request-list.component';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from 'src/app/shared/shared.module';
import { TeacherScoreListComponent } from './components/teacher-score-list/teacher-score-list.component';
import { MatSelectModule } from '@angular/material/select';
import { TeacherReportListComponent } from './components/teacher-report-list/teacher-report-list.component';
import { TeacherReportDetailComponent } from './components/teacher-report-detail/teacher-report-detail.component';
import { PlanService } from './services/plan.service';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CreatePlanDialogComponent } from './components/create-plan-dialog/create-plan-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { CreateNotifyComponent } from './components/create-notify/create-notify.component';
import { NotificationService } from './services/notification.service';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { CreateExerciseDialogComponent } from './components/create-exercise-dialog/create-exercise-dialog.component';
import { ExerciseService } from './services/exercise.service';
import { CreateQuizDialogComponent } from './components/create-quiz-dialog/create-quiz-dialog.component';
import { QuizService } from './services/quiz.service';
import { ClassService } from './services/class.service';
import { AssignmentComponent } from './components/assignment/assignment.component';
import { RequestStudentService } from './services/request-student.service';
import { InviteDialogComponent } from './components/invite-dialog/invite-dialog.component';
import { ClassCourseService } from './services/class-course.service';
import { AddCourseComponent } from './components/add-course/add-course.component';
import { ReportService } from './services/report.service';
import { SubmissionExerciseService } from './services/submission-exercise.service';
import { SubmissionQuizService } from './services/submission-quiz.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { SubmissionExerciseDetailDialogComponent } from './components/submission-exercise-detail-dialog/submission-exercise-detail-dialog.component';

@NgModule({
  declarations: [
    ClassTeacherContainerComponent,
    TeacherCourseManagementComponent,
    StudentManagementComponent,
    TeacherClassDetailComponent,
    TeacherClassPlanComponent,
    TeacherClassExerciseComponent,
    TeacherClassQuizComponent,
    TeacherClassNotificationComponent,
    StudentListComponent,
    RequestListComponent,
    TeacherScoreListComponent,
    TeacherReportListComponent,
    TeacherReportDetailComponent,
    CreatePlanDialogComponent,
    CreateNotifyComponent,
    CreateExerciseDialogComponent,
    CreateQuizDialogComponent,
    AssignmentComponent,
    InviteDialogComponent,
    AddCourseComponent,
    SubmissionExerciseDetailDialogComponent,
  ],
  imports: [
    MatSelectModule,
    MatProgressBarModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    SharedModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDialogModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    ClassTeacherRoutingModule,
    MatTabsModule,
    MatDatepickerModule,
    SharedModule,
  ],
  providers: [
    PlanService,
    NotificationService,
    ExerciseService,
    QuizService,
    ClassService,
    RequestStudentService,
    SubmissionExerciseService,
    SubmissionQuizService,
    ClassCourseService,
    ReportService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class ClassTeacherModule { }
