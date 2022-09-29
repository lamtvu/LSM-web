import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap, timeout } from 'rxjs/operators';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { QuizService } from '../../services/quiz.service';
import { SubmisstionQuizService } from '../../services/submisstion-quiz.service';

@Component({
  selector: 'app-submisstion-quiz',
  templateUrl: './submisstion-quiz.component.html',
  styleUrls: ['./submisstion-quiz.component.scss']
})
export class SubmisstionQuizComponent implements OnInit {
  private $unsubsriber = new Subject();
  $quiz?: Observable<QuizReadDto>;
  quizId?: number;
  setTime?: any;
  time!: Date;
  minutes!: number;
  seconds!: number;
  isTimeOut: boolean = false;

  constructor(
    private _activatedRouter: ActivatedRoute,
    private _submissionService: SubmisstionQuizService,
    private _quizService: QuizService
  ) { }

  ngOnInit(): void {
    this._activatedRouter.params.pipe(takeUntil(this.$unsubsriber)).subscribe(
      data => {
        this.quizId = data.id;
        this.loadQuiz();
      }
    )
  }

  set Seconds(seconds: number) {
    if (seconds < 0) {
      if (this.Minutes == 0) {
        this.minutes = 0;
        this.seconds = 0;
        clearInterval(this.setTime);
      }
      seconds = 59;
      this.Minutes = this.Minutes - 1;
    }
    this.seconds = seconds;
  }

  get Seconds() {
    return this.seconds;
  }

  set Minutes(mutites: number) {
    if (mutites < 0) {
      clearInterval(this.setTime);
      this.isTimeOut = true;
    }
    this.minutes = mutites;
  }

  get Minutes() {
    return this.minutes;
  }

  loadQuiz() {
    if (this.quizId)
      this.$quiz = this._quizService.getQuiz(this.quizId)
        .pipe(takeUntil(this.$unsubsriber), map(res => res.data),
          tap(data => {
            let dateStart = new Date(data.startDate);
            dateStart = new Date(dateStart.setUTCHours(dateStart.getHours() - 7));
            let timespan = (dateStart.getTime() + data.duration * 1000 * 60 - Date.now()) / (1000 * 60);
            if (timespan > 0) {
              this.Minutes = Math.trunc(timespan);
              this.Seconds = Math.trunc((timespan - this.Minutes) * 60);
              this.setTimeRemaining();
              return;
            }
            this.isTimeOut = true;
            this.Minutes = 0;
            this.seconds = 0;
          }));
  }

  setTimeRemaining() {
    this.setTime = setInterval(() => {
      this.Seconds = this.Seconds - 1;
    }, 1000)
  }

  ngOnDestroy(): void {
    if (this.setTime) {
      clearInterval(this.setTime);
    }
    this.$unsubsriber.next();
    this.$unsubsriber.complete();
  }


}
