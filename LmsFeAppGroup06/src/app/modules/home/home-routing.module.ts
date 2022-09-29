import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { NotLoggedGuard } from 'src/app/guards/not-logged.guard';
import { NotFoundComponent } from 'src/app/shared/components/not-found/not-found.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './home.component';
const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [NotLoggedGuard]
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [NotLoggedGuard]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class HomeRoutingModule { }

