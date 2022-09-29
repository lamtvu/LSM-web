import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseListComponent } from './components/CourseManagement/course-list/course-list.component';
import { IntrustorContainerComponent } from './components/intrustor-container/intrustor-container.component';
import { IntructorRoutingModule } from './intructor-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { CourseContainerComponent } from './components/CourseManagement/course-container/course-container.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CourseCreateComponent } from './components/CourseManagement/course-create/course-create.component';
import { CourseDetailComponent } from './components/CourseManagement/course-detail/course-detail.component';
import { CourseEditComponent } from './components/CourseManagement/course-edit/course-edit.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { EditContentComponent } from './components/CourseManagement/edit-content/edit-content.component'
import { MatExpansionModule } from '@angular/material/expansion';
import { CourseDeleteComponent } from './components/CourseManagement/course-delete/course-delete.component';
import { CourseAnalysisComponent } from './components/CourseManagement/course-analysis/course-analysis.component';
import { ChartsModule } from '@rinminase/ng-charts';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CourseReviewComponent } from './components/CourseManagement/course-review/course-review.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { CourseService } from './services/course.service';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SectionService } from './services/section.service';
import { ContentService } from './services/content.service';
import { ChangeSectionNameComponent } from './components/CourseManagement/change-section-name/change-section-name.component';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


@NgModule({
  declarations: [
    IntrustorContainerComponent,
    CourseListComponent,
    CourseContainerComponent,
    CourseCreateComponent,
    CourseDetailComponent,
    CourseEditComponent,
    EditContentComponent,
    CourseDeleteComponent,
    CourseAnalysisComponent,
    CourseReviewComponent,
    ChangeSectionNameComponent
  ],
  imports: [
    CommonModule,
    IntructorRoutingModule,
    SharedModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatTabsModule,
    MatInputModule,
    MatDialogModule,
    MatExpansionModule,
    ChartsModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    MatPaginatorModule,
    MatProgressSpinnerModule
  ],
  providers: [
    CourseService,
    SectionService,
    ContentService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class InstructorModule { }
