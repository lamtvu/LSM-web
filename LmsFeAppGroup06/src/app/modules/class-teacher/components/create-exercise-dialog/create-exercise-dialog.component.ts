import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Data } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ExerciseReadDto } from '../../../../Dto/ExerciseDto';
import { ClassService } from '../../services/class.service';
import { ExerciseService } from '../../services/exercise.service';

@Component({
  selector: 'app-create-exercise-dialog',
  templateUrl: './create-exercise-dialog.component.html',
  styleUrls: ['./create-exercise-dialog.component.scss']
})
export class CreateExerciseDialogComponent implements OnInit {
  private $unsubscriber = new Subject();
  stateName: 'IDE' | 'LOADING' | 'ERROR' = 'IDE'
  dialogState = { state: this.stateName, messager: this.stateName };

  exerciseForm = this._formBuilder.group({
    name: ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50),
    ]],
    description: ['', [
      Validators.required,
    ]],
    dueDate: ['', [
      Validators.required
    ]],
    dueTime: ['', [
      Validators.required,
      Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]
    ]
  })

  errorHandling(controlName: string, errorName: string) {
    return this.exerciseForm.controls[controlName].hasError(errorName);
  }

  constructor(
    private _formBuilder: FormBuilder,
    private _exerciseService: ExerciseService,
    private _dialogRef: MatDialogRef<CreateExerciseDialogComponent>,
    private _classService: ClassService,
    @Inject(MAT_DIALOG_DATA) public exercise: ExerciseReadDto
  ) {

  }

  ngOnInit(): void {
    if (!this.exercise) return;
    const timespan = Date.parse(this.exercise.dueDate);
    let dueTime = '12:30';
    if (!isNaN(timespan)) {
      const date = new Date(timespan);
      const timeString = date.toTimeString().split(' ')[0];
      const times = timeString.split(':');
      dueTime = times[0] + ':' + times[1];
    }
    this.exerciseForm.patchValue({...this.exercise, dueTime: dueTime})
  }

  clickHandling(): void {
    if (this.exercise) {
      this.editHandling();
      return;
    }
    this.createHandling();
  }

  createHandling(): void {
    if (!this.exerciseForm.valid || !this._classService.classId)
      return;
    const dateString = this.exerciseForm.controls['dueDate'].value;
    const dueDate = dateString as Data;
    dueDate.setUTCDate(dueDate.getDate());

    this.dialogState.state = 'LOADING';
    this._exerciseService.createExercise(this._classService.classId, { ...this.exerciseForm.value, dueDate: dueDate })
      .pipe(takeUntil(this.$unsubscriber)).subscribe(
        res => {
          this.dialogState.state = 'IDE';
          this._dialogRef.close(true);
        },
        res => {
          this.dialogState.state = 'ERROR';
          this.dialogState.messager = res.error.messager;
          console.log(res.error)
        }
      )
  }

  editHandling(): void {
    if (!this.exerciseForm.valid)
      return;
    const dateString = this.exerciseForm.controls['dueDate'].value;
    const dueDate = new Date(dateString);
    dueDate.setUTCDate(dueDate.getDate());

    this.dialogState.state = 'LOADING';
    this._exerciseService.editExercise(this.exercise.id, { ...this.exerciseForm.value, dueDate: dueDate })
      .pipe(takeUntil(this.$unsubscriber)).subscribe(
        res => {
          this.dialogState.state = 'IDE';
          this._dialogRef.close(true);
        }, res => {
          console.log(res.error)
          this.dialogState.state = 'ERROR';
          this.dialogState.messager = res.error.messager;
        }
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
