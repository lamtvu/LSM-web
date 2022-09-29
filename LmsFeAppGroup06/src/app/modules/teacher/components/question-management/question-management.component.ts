import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { questionService } from '../../services/question.service';
import { QuizService } from '../../services/quiz.service';

@Component({
  selector: 'app-question-management',
  templateUrl: './question-management.component.html',
  styleUrls: ['./question-management.component.scss']
})
export class QuestionManagementComponent implements OnInit {
  private $unsubscriber = new Subject();
  $quiz?: Observable<QuizReadDto>;
  $questions?: Observable<{ id: number }[]>
  currentQuestionId?: number;
  isLoading: boolean = false;

  constructor(
    private _quizService: QuizService,
    private _activeRoute: ActivatedRoute,
    private _questionSercice: questionService
  ) { }

  ngOnInit(): void {
    this._activeRoute.params.pipe(takeUntil(this.$unsubscriber))
      .subscribe(
        data => {
          this._quizService.setQuizId(data.id);
          this.loadQuiz();
          this.loadQuestion(0);
        }
      )
  }

  loadQuiz() {
    if (this._quizService.quizId)
      this.$quiz = this._quizService.getQuiz(this._quizService.quizId)
        .pipe(map(res => res.data))
  }

  loadCreateQuestion() {
    this.currentQuestionId = 0;
  }

  loadQuestion(questionId: number) {
    if (!this._quizService.quizId) return;
    this.isLoading = true;
    this.$questions = this._questionSercice
      .getQuestions(this._quizService.quizId)
      .pipe(map(res => res.data),
        tap(res => {
          this.isLoading = false
          this.currentQuestionId = questionId;
          console.log(res)
        },
          res => {
            this.isLoading = false
            console.log(res)
          }))
  }

  deleteQuestion(id: number) {
    this.loadQuestion(0);
  }


  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

  onChooseQuestion(questionId: number) {
    this.currentQuestionId = questionId;
  }

}
