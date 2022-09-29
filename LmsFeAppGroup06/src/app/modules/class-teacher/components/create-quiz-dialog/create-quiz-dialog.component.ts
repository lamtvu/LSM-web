import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { QuizService } from 'src/app/modules/class-teacher/services/quiz.service';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { ClassService } from '../../services/class.service';

@Component({
  selector: 'app-create-quiz-dialog',
  templateUrl: './create-quiz-dialog.component.html',
  styleUrls: ['./create-quiz-dialog.component.scss']
})
export class CreateQuizDialogComponent implements OnInit {
  private $unsubscribe = new Subject();
  stateName: 'IDE' | 'LOADING' | 'ERROR' = 'IDE'
  dialogState = { state: this.stateName, message: '' };

  quizForm = this._formBuilder.group({
    name: ['', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(50)
    ]],
    description: [ '',
      Validators.required],
    startDate: [
      this.quiz?.startDate || '',
      Validators.required],
    startTime: ['', [
        Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$'),
        Validators.required,
      ]],
    duration: ['', [
        Validators.required,
        Validators.min(5),
      ]],
  })
  
  constructor(
    private _formBuilder: FormBuilder,
    private _classService: ClassService,
    private _dialogRef: MatDialogRef<CreateQuizDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public quiz: QuizReadDto,
    private _quizService: QuizService
  ) { }

  ngOnInit(): void {
    if(!this.quiz) return;
    const timespan = Date.parse(this.quiz.startDate);
    let startTime = '12:30';
    if (!isNaN(timespan)) {
      const date = new Date(timespan);
      const timeString = date.toTimeString().split(' ')[0];
      const times = timeString.split(':');
      startTime = times[0] + ':' + times[1];
    }
    if(this.quiz){
      this.quizForm.patchValue({...this.quiz, startTime: startTime});
    }
  }

  errorHandling(controlName: string, errorName: string) {
    return this.quizForm.controls[controlName].hasError(errorName);
  }

  createHandling() {
    if (!this.quizForm.valid)
      return;
    const dateString = new Date(this.quizForm.controls['startDate'].value);
    const startDate = dateString.setUTCDate(dateString.getDate());
    this.dialogState.state = 'LOADING';
    if (this._classService.classId)
      this._quizService.createQuiz(this._classService.classId, {
        ...this.quizForm.value,
        startDate: new Date(startDate).toJSON()
      }).pipe(
        takeUntil(this.$unsubscribe)).subscribe(
          res => {
            this._dialogRef.close(true);
            this.dialogState.state = 'IDE';
          },
          res => {
            console.log(res.error)
            this.dialogState.state = 'ERROR';
            this.dialogState.message = res.error.messager;
          }
        )
  }

  editHandling() {
    console.log(this.quizForm)
    if (!this.quizForm.valid)
      return;
    const dateString = new Date(this.quizForm.controls['startDate'].value);
    const startDate = dateString.setUTCDate(dateString.getDate());
    this.dialogState.state = 'LOADING';
    this._quizService.editQuiz(this.quiz.id, {
      ...this.quizForm.value,
      startDate: new Date(startDate).toJSON()
    }).pipe(
      takeUntil(this.$unsubscribe)).subscribe(
        res => {
          this._dialogRef.close(true);
          this.dialogState.state = 'IDE';
        },
        res => {
          console.log(res.error)
          this.dialogState.state = 'ERROR';
          this.dialogState.message = res.error.messager;
        }
      )
  }

  onClick(): void {
    if (!this.quiz) {
      this.createHandling();
      return;
    }
    this.editHandling();
  }
}
