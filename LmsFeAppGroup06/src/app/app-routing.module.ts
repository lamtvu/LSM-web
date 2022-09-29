import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';
import { InstructorGuard } from './guards/instructor.guard';
import { NotLoggedGuard } from './guards/not-logged.guard';
import { StudentGuard } from './guards/student.guard';
import { TeacherGuard } from './guards/teacher.guard';
import { HomeComponent } from './modules/home/home.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

const routes: Routes = [
  {
    path:'',
    redirectTo:'home',
    pathMatch:'full'
  },
  {
    path: 'teacher',
    canActivate: [AuthGuard, TeacherGuard],
    loadChildren: () => import('./modules/teacher/teacher.module').then(m => m.TeacherModule)
  },
  {
    path: 'student',
    canActivate: [AuthGuard, StudentGuard],
    loadChildren: () => import('./modules/student/student.module').then(m => m.StudentModule)
  },
  {
    path: 'instructor',
    canActivate: [AuthGuard, InstructorGuard],
    loadChildren: () => import('./modules/instructor/instructor.module').then(m => m.InstructorModule)
  },
  {
    path: 'admin',
    canActivate: [AuthGuard, AdminGuard],
    loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule)
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
