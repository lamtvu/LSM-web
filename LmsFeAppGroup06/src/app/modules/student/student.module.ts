import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { StudentRoutingModule } from './student-routing.module';
import { StudentContainerComponent } from './student-container/student-container.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MyClassComponent } from './components/my-class/my-class.component';
import { ClassCartComponent } from './components/class-cart/class-cart.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { StudentService } from './services/student.service';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SubmisstionQuizComponent } from './components/submisstion-quiz/submisstion-quiz.component';
import { QuestionDetailComponent } from './components/question-detail/question-detail.component';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { QuizService } from './services/quiz.service';
import { QuestionService } from './services/question.service';
import { MatRadioModule } from '@angular/material/radio';
import { SubmisstionQuizService } from './services/submisstion-quiz.service';
import { StudentRequestComponent } from './components/student-request/student-request.component';
import { StudentRequestDialogComponent } from './components/student-request-dialog/student-request-dialog.component';
import { RequestService } from './services/request.service';



@NgModule({
  declarations: [
    StudentContainerComponent,
    MyClassComponent,
    ClassCartComponent,
    SubmisstionQuizComponent,
    QuestionDetailComponent,
    StudentRequestComponent,
    StudentRequestDialogComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    StudentRoutingModule,
    MatIconModule,
    SharedModule,
    MatProgressBarModule,
    MatPaginatorModule,
    MatInputModule,
    MatDialogModule,
    MatCardModule,
    MatTabsModule,
    MatCheckboxModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatInputModule,
    MatRadioModule,
    MatProgressSpinnerModule,
  ],
  providers:[
    StudentService,
    QuizService,
    SubmisstionQuizService,
    QuestionService,
    RequestService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class StudentModule { }
