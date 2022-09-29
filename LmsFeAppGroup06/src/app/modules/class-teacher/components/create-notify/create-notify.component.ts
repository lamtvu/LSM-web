import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AnnouncenmentReadDto } from '../../../../Dto/AnnouncenmentDto';
import { ClassService } from '../../services/class.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-create-notify',
  templateUrl: './create-notify.component.html',
  styleUrls: ['./create-notify.component.scss']
})
export class CreateNotifyComponent implements OnInit {
  private $unsubsriber = new Subject();
  stateName: 'IDE' | 'LOADING' | 'ERROR' | 'SUCCESS' = 'IDE';
  createState = { state: this.stateName, message: '' };

  notifyForm = this._formBuilder.group({
    'title': ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50)
    ]],
    'content': ['', [
      Validators.required,
    ]]
  })

  constructor(
    @Inject(MAT_DIALOG_DATA) public notify: AnnouncenmentReadDto,
    private _dialogRef: MatDialogRef<CreateNotifyComponent>,
    private _formBuilder: FormBuilder,
    private _notificationService: NotificationService,
    private _classService: ClassService
  ) {
  }

  ngOnInit(): void {
    if (this.notify) {
      this.notifyForm.patchValue(this.notify);
    }
  }

  clickHandling() {
    if (this.notify) {
      this.editHandling();
      return;
    }
    this.createHandling();
  }

  createHandling() {
    console.log(this.notifyForm.value)
    if (!this.notifyForm.valid)
      return;
    this.createState.state = 'LOADING';
    if (this._classService.classId)
      this._notificationService.createNotification(this._classService.classId,
        this.notifyForm.value).pipe(takeUntil(this.$unsubsriber))
        .subscribe(
          res => {
            this.createState.state = 'SUCCESS';
            this._dialogRef.close(true);
          },
          res => {
            console.log(res);
            this.createState.state = 'ERROR';
            this.createState.message = res.error.message;
          }
        )
  }

  editHandling() {
    if (!this.notifyForm.valid)
      return;
    this.createState.state = 'LOADING';
    this._notificationService.editNotification(this.notify.id,
      this.notifyForm.value).pipe(takeUntil(this.$unsubsriber))
      .subscribe(
        res => {
          this.createState.state = 'SUCCESS';
          this._dialogRef.close(true);
        },
        res => {
          console.log(res);
          this.createState.state = 'ERROR';
          this.createState.message = res.error.message;
        }
      )
  }

  errorHandling(controlName: string, errorName: string) {
    return this.notifyForm.controls[controlName].hasError(errorName);
  }

  ngOnDestroy(): void {
    this.$unsubsriber.next();
    this.$unsubsriber.complete();
  }


}
