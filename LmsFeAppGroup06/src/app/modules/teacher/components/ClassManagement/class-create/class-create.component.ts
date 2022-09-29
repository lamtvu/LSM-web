import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TeacherService } from '../../../services/teacher.service';

@Component({
  selector: 'app-class-create',
  templateUrl: './class-create.component.html',
  styleUrls: ['./class-create.component.scss']
})
export class ClassCreateComponent implements OnInit, OnDestroy {
  private $unscrupcriber = new Subject();

  stateName: 'IDE' | 'LOADING' | 'ERROR' = 'IDE'
  createState = { state: this.stateName, message: '' };
  createForm = this._formBuilder.group({
    'name': ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50)]],
    'description': ['']
  })

  constructor(
    public dialogRef: MatDialogRef<ClassCreateComponent>,
    public _httpClientService: HttpClient,
    private _formBuilder: FormBuilder,
    private _teacherService: TeacherService
  ) { }

  ngOnInit(): void {

  }

  errorHandling(ControlName: string, errorName: string):boolean {
    return this.createForm.controls[ControlName].hasError(errorName);
  }

  createHandling() {
    if (!this.createForm.valid)
      return;
    this.createState.state = 'LOADING';
    this._teacherService.createClass(this.createForm.value).pipe(takeUntil(this.$unscrupcriber))
      .subscribe(
        res => {
          this.dialogRef.close(true);
        },
        res => {
          this.createState.state = 'ERROR',
          this.createState.message = res.error.messager;
        }
      )
  }

  ngOnDestroy(): void {
    this.$unscrupcriber.next();
    this.$unscrupcriber.complete();
  }
}
