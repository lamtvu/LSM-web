import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared/services/user.service';
import { UserReadDto } from '../../../../Dto/userDto';

@Component({
  selector: 'app-change-information',
  templateUrl: './change-information.component.html',
  styleUrls: ['./change-information.component.scss']
})
export class ChangeInformationComponent implements OnInit {

  private unsubscribe$ = new Subject<void>();
  stateName: 'IDE' | 'LOADING' | 'ERROR' | 'SUCCESS' = 'IDE';
  changeInfoState = { state: this.stateName, message: '' };

  changeInfoForm = this._formBuilder.group({
    'gender': ['Male', [Validators.required]],
    'fullName': [this.userData.fullName, [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50)]
    ],
    'phone': [this.userData.phone, [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(15),
      Validators.pattern('[0-9]{9,12}')]
    ]
  })

  public fullName: string = '';
  public phone: string = '';
  isLoading = false;

  constructor(
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _routerService: Router,
    public dialogRef: MatDialogRef<ChangeInformationComponent>,
    @Inject(MAT_DIALOG_DATA) public userData: UserReadDto
  ) { }

  ngOnInit(): void {
  }

  public errorHandling = (control: string, error: string) => {
    return this.changeInfoForm.controls[control].hasError(error);
  }

  handlerChangeInfor() {
    // console.log(this.changeInfoForm.value)
    if (!this.changeInfoForm.valid) {
      return;
    }
    this.isLoading = true;
    this._userService.changeInfo(this.changeInfoForm.value)
      .pipe(takeUntil(this.unsubscribe$)).subscribe(
        res => {
          // console.log('success')
          this.isLoading = false;
          this._userService.refreshList();
          this.dialogRef.close();
          //router
        },
        res => {
          console.log(res.error.messager);
          console.log(res.data);
        }
      )
  }


  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.unsubscribe();
  }


}
