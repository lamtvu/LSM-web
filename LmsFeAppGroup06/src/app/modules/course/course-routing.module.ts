import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { AuthInterceptor } from 'src/app/interceptors/auth.interceptor';
import { CourseContentComponent } from './components/course-content/course-content.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';
import { CourseContainerComponent } from './course-container/course-container.component';
import { CourseService } from './services/course.service';
import { SectionService } from './services/section.service';

const routes: Routes = [
  {
    path: ':id',
    component: CourseContainerComponent,
    children: [
      {
        path:'',
        redirectTo: 'course-detail',
        pathMatch: 'full'
      },
      {
        path: 'course-detail',
        component: CourseDetailComponent
      },
      {
        path: 'course-content',
        component: CourseContentComponent
      }
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule
  ]
})
export class CourseRoutingModule { }

