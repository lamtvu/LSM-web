import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AnnouncenmentReadDto } from '../../../../Dto/AnnouncenmentDto';
import { ClassService } from '../../services/class.service';
import { PlanService } from '../../services/plan.service';

@Component({
  selector: 'app-create-plan-dialog',
  templateUrl: './create-plan-dialog.component.html',
  styleUrls: ['./create-plan-dialog.component.scss']
})
export class CreatePlanDialogComponent implements OnInit, OnDestroy {
  private $unsubsriber = new Subject();
  stateName: 'IDE' | 'LOADING' | 'ERROR' | 'SUCCESS' = 'IDE';
  createState = { state: this.stateName, message: '' };

  planForm = this._formBuilder.group({
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
    @Inject(MAT_DIALOG_DATA) public plan: AnnouncenmentReadDto,
    private _dialogRef: MatDialogRef<CreatePlanDialogComponent>,
    private _formBuilder: FormBuilder,
    private _planService: PlanService,
    private _classService: ClassService
  ) {
  }

  ngOnInit(): void {
    if (this.plan) {
      this.planForm.patchValue(this.plan);
    }
  }

  clickHandling() {
    if (this.plan) {
      this.editHandling();
      return;
    }
    this.createHandling();
  }

  createHandling() {
    if (!this.planForm.valid)
      return;
    this.createState.state = 'LOADING';
    if (this._classService.classId)
      this._planService.createPlan(this._classService.classId,
        this.planForm.value).pipe(takeUntil(this.$unsubsriber))
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
    if (!this.planForm.valid)
      return;
    this.createState.state = 'LOADING';
    this._planService.editPlans(this.plan.id,
      this.planForm.value).pipe(takeUntil(this.$unsubsriber))
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
    return this.planForm.controls[controlName].hasError(errorName);
  }

  ngOnDestroy(): void {
    this.$unsubsriber.next();
    this.$unsubsriber.complete();
  }

}
