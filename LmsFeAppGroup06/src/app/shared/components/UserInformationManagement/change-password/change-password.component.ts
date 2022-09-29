import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared/services/user.service';


@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  private $unsubcriber = new Subject();
  isLoading = false;
  public userRole?: string;
  public linkback?: string;
  stateName: 'IDE' | 'LOADING' | 'ERROR' | 'SUCCESS' = 'IDE';
  changeState = { state: this.stateName, message: '' };

 changeForm = this._formBuilder.group({
    oldpass: ['', [
      Validators.required,
      Validators.minLength(6),
    ]],
    newpass: ['', [
      Validators.required,
      Validators.minLength(6),
    ]],
    confirmpass: ['', [
      Validators.required,
      Validators.minLength(6),
    ]]
  })

  errorHandling(controlName: string, errorName: string) {
    return this.changeForm.controls[controlName].hasError(errorName);
  }

  constructor(
    public _formBuilder: FormBuilder,
    private _userService: UserService,
    private _routerService: Router,
    private activateRouter: ActivatedRoute
  ) {

  }

  ngOnInit(): void {
  }

  changeHandling(){
    if(!this.changeForm.valid){
      this.changeState.state = 'ERROR';
      return;
    }
    this.changeState.state = 'LOADING';
    this._userService.changePassword(this.changeForm.value).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res => {
          this.changeState.state = 'SUCCESS';
          this.changeState.message='Change password successfully';
          this._routerService.navigate(['../'],{relativeTo: this.activateRouter});
      },
      res => {
        this.changeState.state = 'ERROR';
        this.changeState.message = res.error.messager;
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }

}
