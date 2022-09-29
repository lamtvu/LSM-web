import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { TeacherContainerComponent } from './components/teacher-container/teacher-container.component';
import { ClassCreateComponent } from './components/ClassManagement/class-create/class-create.component';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ClassListComponent } from './components/ClassManagement/class-list/class-list.component';
import { TeacherRoutingModule } from './teacher-routing.module';
import { ClassDeleteComponent } from './components/ClassManagement/class-delete/class-delete.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ClassManagementComponent } from './components/ClassManagement/class-management/class-management.component';
import { ClassInformationComponent } from './components/ClassManagement/class-information/class-information.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { TeacherService } from './services/teacher.service';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule, MatSpinner } from '@angular/material/progress-spinner';
import { QuestionManagementComponent } from './components/question-management/question-management.component';
import { QuestionDetailComponent } from './components/question-detail/question-detail.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { questionService } from './services/question.service';
import { QuizService } from './services/quiz.service';

@NgModule({
  declarations: [
    TeacherContainerComponent,
    ClassCreateComponent,
    ClassListComponent,
    ClassDeleteComponent,
    ClassManagementComponent,
    ClassInformationComponent,
    QuestionManagementComponent,
    QuestionDetailComponent,
  ],
  imports: [
    CommonModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    FormsModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    TeacherRoutingModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    SharedModule,
  ],
  providers:[
    TeacherService,
    questionService,
    QuizService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class TeacherModule { }
