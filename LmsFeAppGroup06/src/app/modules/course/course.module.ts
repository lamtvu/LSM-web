import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { SharedModule } from 'src/app/shared/shared.module';
import { CourseRoutingModule } from './course-routing.module';
import { CourseContainerComponent } from './course-container/course-container.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDialogModule } from '@angular/material/dialog';
import { CourseReviewComponent } from './components/course-review/course-review.component';
import { CourseContentComponent } from './components/course-content/course-content.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CourseContentDialogComponent } from './components/course-content-dialog/course-content-dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { CourseService } from './services/course.service';
import { SectionService } from './services/section.service';



@NgModule({
  declarations: [
    CourseContainerComponent,
    CourseDetailComponent,
    CourseReviewComponent,
    CourseContentComponent,
    CourseContentDialogComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    CourseRoutingModule,
    MatCardModule,
    MatButtonModule,
    MatTabsModule,
    MatDialogModule,
    MatExpansionModule,
    MatCheckboxModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    CourseService,
    SectionService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class CourseModule { }
