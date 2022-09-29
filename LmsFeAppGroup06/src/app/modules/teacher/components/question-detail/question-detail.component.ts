import { ThrowStmt } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output, SimpleChange } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { questionService } from '../../services/question.service';
import { QuizService } from '../../services/quiz.service';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.scss']
})
export class QuestionDetailComponent implements OnInit {
  @Input() questionId?: number;
  @Output('onsave') onSaveHandling = new EventEmitter<number>();
  @Output('onDelete') onDeleteHandling = new EventEmitter<number>();
  isLoading: boolean = false;

  $unsubscriber = new Subject();
  questionForm = this._formBuilder.group({
    content: ['', Validators.required],
    answers: this._formBuilder.array([])
  })

  get answers() {
    return this.questionForm.controls['answers'] as FormArray
  }

  constructor(
    private _formBuilder: FormBuilder,
    private _questionService: questionService,
    private _matDialog: MatDialog,
    private _quizService: QuizService
  ) { }

  ngOnInit(): void {
    this.loadQuestion();
  }

  ngOnChanges(changes: SimpleChange): void {
    this.loadQuestion();
  }

  loadQuestion() {
    if (this.questionId) {
      this.isLoading = true
      this._questionService.getQuestion(this.questionId)
        .pipe(takeUntil(this.$unsubscriber), map(res => res.data)).subscribe(data => {
          this.isLoading = false;
          this.questionForm.patchValue(data);
          const answerGroupForms = data.answers.map(answer => this._formBuilder.group(answer));
          const answerArrayForm = this._formBuilder.array(answerGroupForms);
          this.questionForm.setControl('answers', answerArrayForm);
        }, res => this.isLoading = false);
      return;
    }
    const answerArrayForm = this._formBuilder.array([]);
    this.questionForm.setControl('answers', answerArrayForm);
    this.questionForm.reset();
  }

  deleteHandling() {
    const deleteDialog = this._matDialog.open(DeleteDialogComponent, { data: { title: 'Delete Question', deleteName: 'this question' } });
    deleteDialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      result => {
        if (!result)
          return;
        if (!this.questionId)
          return;
        this.isLoading = true;
        this._questionService.deleteQuestion(this.questionId)
          .pipe(takeUntil(this.$unsubscriber)).subscribe(res => {
            this.isLoading = false;
            this.onDeleteHandling.emit(this.questionId);
          }, res => this.isLoading = false);
      })
  }

  editHandling() {
    if (!this.questionId) return;
    this.isLoading = true;
    this._questionService.editQuestion(this.questionId, this.questionForm.value).pipe(
      takeUntil(this.$unsubscriber)).subscribe(res => {
        this.isLoading = false;
        this.onSaveHandling.emit(res.data.id);
      }, res => {
        console.log(res)
        this.isLoading = false;
      })
  }

  addAnswer() {
    const fg = this._formBuilder.group({
      content: ['', Validators.required],
      isCorrect: [false, Validators.required]
    })
    this.answers.push(fg);
  }

  removeAnswer(index: number) {
    this.answers.removeAt(index);
  }

  saveHandling(): void {
    if (!this.questionForm.valid) return;
    if (!this.questionId) {
      this.createHandling();
      return;
    }
    this.editHandling();
  }

  createHandling(): void {
    if (!this._quizService.quizId) return;
    this.isLoading = true;
    this._questionService.createQuestion(this._quizService.quizId, this.questionForm.value).pipe(
      takeUntil(this.$unsubscriber)).subscribe(res => {
        this.isLoading = false;
        this.onSaveHandling.emit(0);
        this.loadQuestion();
      }, res => {
        console.log(res)
      })
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
