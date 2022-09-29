import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/services/auth.service';
import { equalValueValidator } from '../../../../shared/custom-validators/equal-value.validator';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private unsubscribe$ = new Subject<void>();
  stateName: 'IDE' | 'LOADING' | 'ERROR' | 'SUCCESS' = 'IDE';
  loginState = { state: this.stateName, message: '' };
  registerState = { state: this.stateName, message: '' };

  loginForm = this._formBuilder.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  })

  registerForm = this._formBuilder.group({
    'username': ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50)]
    ],
    'password': ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50)]
    ],
    'confirmPassword': ['', Validators.required],
    'email': ['', [Validators.required, Validators.email]],
    'gender': ['Male', [Validators.required]],
    'fullname': ['', Validators.required],
    'phone': ['', [Validators.required, Validators.pattern('[0-9]{9,12}')]],
  }, { validators: [equalValueValidator('password', 'confirmPassword')] })

  constructor(
    private _formBuilder: FormBuilder,
    private _authService: AuthService,
    private _routerService: Router
  ) { }

  ngOnInit(): void {
  }

  public errorHandling = (control: string, error: string) => {
    return this.registerForm.controls[control].hasError(error);
  }
  
  handlerLogin() {
    if (!this.loginForm.valid) {
      this.loginState.state = 'ERROR';
      this.loginState.message = 'Enter your username and password';
      return;
    }
    this.loginState.state = 'LOADING'
    this._authService.login(this.loginForm.value)
      .pipe(takeUntil(this.unsubscribe$)).subscribe(
        res => {
          localStorage.setItem('token', res.data.token);
          localStorage.setItem('role', res.data.role);
          console.log(localStorage.getItem('role'));
          this._routerService.navigate([`/${res.data.role.toLowerCase()}`]);
        },
        res => {
          this.loginState.state = 'ERROR'
          this.loginState.message = res.error.messager;
        }
      )
  }

  handlerRegister() {
    console.log(this.registerForm)
    if (!this.registerForm.valid) {
      return
    }
    this.registerState.state = 'LOADING'
    this._authService.register(this.registerForm.value)
      .pipe(takeUntil(this.unsubscribe$)).subscribe(
        res => {
          this._authService.getVerifyGmail(this.registerForm.value.email).subscribe(
            res => {
              this.registerForm.reset();
              this.registerState.state = 'SUCCESS';
            },
            res => {
              this.registerState.state = 'ERROR';
              console.log(res)
            }
          )
        },
        res => {
          this.registerState.state = 'ERROR';
          this.registerState.message = res.error.messager;
        }
      )
  }


  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.unsubscribe();
  }

}
