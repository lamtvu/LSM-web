import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { SubmissionQuizReaDto } from '../../../../Dto/submissionDto';
import { QuizService } from '../../services/quiz.service';
import { SubmisstionQuizService } from '../../services/submisstion-quiz.service';

@Component({
  selector: 'app-quiz-detail',
  templateUrl: './quiz-detail.component.html',
  styleUrls: ['./quiz-detail.component.scss']
})
export class QuizDetailComponent implements OnInit {
  $unsubscriber = new Subject();
  quiz?: QuizReadDto;
  $submission?: Observable<SubmissionQuizReaDto>;
  $quizId?: number;

  stateName: 'LOADING' | 'ERROR' | 'IDE' = 'IDE';
  submissionState = { state: this.stateName, message: '' };

  isLoadingQuiz: boolean = false;
  isLoadingSubmission: boolean = false;

  constructor(
    private _activateRouter: ActivatedRoute,
    private _quizService: QuizService,
    private _submisstionQuizService: SubmisstionQuizService
  ) { }

  ngOnInit(): void {
    this._activateRouter.params.pipe(takeUntil(this.$unsubscriber))
      .subscribe(params => {
        this.$quizId = params.id;
        this.loadQuiz();
        this.loadSubmission();
      })
  }

  loadQuiz() {
    if (!this.$quizId) return;
    this.isLoadingQuiz = true;
    this._quizService.getQuiz(this.$quizId).pipe(
      map(res => res.data), takeUntil(this.$unsubscriber)).subscribe(
        data => {
          this.quiz = data
          this.isLoadingQuiz = false;
        }, res => {
          this.isLoadingQuiz = false;
        }
      )
  }

  loadSubmission() {
    if (!this.$quizId) return;
    this.isLoadingSubmission = true;
    this.$submission = this._submisstionQuizService.getSubmission(this.$quizId).pipe(
      map(res => res.data), takeUntil(this.$unsubscriber), tap(res => {
        this.isLoadingSubmission = false;
      }, res => {
        this.isLoadingSubmission = false;
      })
    );
  }

  createSubmission() {
    if (!this.$quizId) return;
    this.submissionState.state = 'LOADING'
    this._submisstionQuizService.addSubmission(this.$quizId).pipe(
      takeUntil(this.$unsubscriber)).subscribe(
        res => {
          this.submissionState.state = 'IDE';
          this.loadSubmission();
        },
        res => {
          this.submissionState.state = 'ERROR';
          this.submissionState.message = res.error.messager;
        }
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
