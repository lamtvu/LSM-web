import { Component, ElementRef, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { map, take, takeUntil } from 'rxjs/operators';
import { QuestionReadDto } from '../../../../Dto/QuestionDto';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { SubmissionQuizReaDto } from '../../../../Dto/submissionDto';
import { QuestionService } from '../../services/question.service';
import { SubmisstionQuizService } from '../../services/submisstion-quiz.service';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.scss']
})
export class QuestionDetailComponent implements OnInit {
  private $unsubscriver = new Subject();
  @Input() quiz?: QuizReadDto;

  quizId?: number;
  questions?: { id: number }[];
  submission?: SubmissionQuizReaDto;
  currentQuestionId?: number;
  currentQuestion?: QuestionReadDto;

  isLoadQuestion: boolean = false;
  isSubmitting: boolean = false;
  errorState: { isError: boolean, message: string } = { isError: false, message: '' };

  answerInput = this._formBuilder.group({
    answer: ['', Validators.required],
  })

  constructor(
    private _activatedRouter: ActivatedRoute,
    private _submissionService: SubmisstionQuizService,
    private _questionService: QuestionService,
    private _formBuilder: FormBuilder,
    private _routerService: Router,
  ) { }

  ngOnInit(): void {
    this._activatedRouter.params.pipe(takeUntil(this.$unsubscriver))
      .subscribe(
        data => {
          this.quizId = data.id;
          this.loadQuestions();
          this.loadingSubmission();
        }
      )
  }

  loadQuestions() {
    if (this.quizId) {
      this._questionService.getQuestions(this.quizId).pipe(takeUntil(this.$unsubscriver),
        map(res => res.data)).subscribe(
          data => {
            this.questions = data
            if (this.questions[0]) {
              this.currentQuestionId = this.questions[0].id;
              this.loadQuestion();
            }
          }
        )
    }
  }

  submitAnswer() {
    if (!this.answerInput.valid || !this.submission) return;
    this._submissionService.submitAnswer(this.submission.id, this.answerInput.controls['answer'].value)
      .pipe(takeUntil(this.$unsubscriver)).subscribe(
        res => {
          this.loadingSubmission();
        }, res => {
          this.errorState.isError = true;
          this.errorState.message = res.error.messager;
        }
      );
  }

  submitQuiz() {
    if (!this.submission) return;
    if (this.answerInput.valid) {
      this._submissionService.submitAnswer(this.submission.id, this.answerInput.controls['answer'].value)
        .pipe(takeUntil(this.$unsubscriver)).subscribe(
          res => {
            if (this.submission) {
              this.isSubmitting = true;
              this._submissionService.submitQuiz(this.submission.id).pipe(takeUntil(this.$unsubscriver))
                .subscribe(res => {
                  this.isSubmitting = false;
                  this._routerService.navigate(['/student/class', this.quiz?.classId, 'class-detail', 'quiz', 'detail', this.quiz?.id])
                }, res => {
                  this.isSubmitting = false;
                  this._routerService.navigate(['/student/class', this.quiz?.classId, 'class-detail', 'quiz', 'detail', this.quiz?.id])
                })
            }
          }, res => {
            this.errorState.isError = true;
            this.errorState.message = res.error.messager;
          }
        );
      return;
    }
    this._submissionService.submitQuiz(this.submission.id).pipe(takeUntil(this.$unsubscriver))
      .subscribe(res => {
        this.isSubmitting = false;
        this._routerService.navigate(['/student/class', this.quiz?.classId, 'class-detail', 'quiz', 'detail', this.quiz?.id])
      }, res => {
        this.isSubmitting = false;
        this._routerService.navigate(['/student/class', this.quiz?.classId, 'class-detail', 'quiz', 'detail', this.quiz?.id])
      })
  }


  loadQuestion() {
    if (this.currentQuestionId) {
      this.isLoadQuestion = true;
      this.submitAnswer();
      this.answerInput.reset();
      this._questionService.getQuestion(this.currentQuestionId).pipe(takeUntil(this.$unsubscriver),
        map(res => res.data)).subscribe(
          data => {
            this.isLoadQuestion = false
            this.currentQuestion = data;
          }, res => this.isLoadQuestion = false)
    }
  }

  loadingSubmission() {
    if (this.quizId) {
      this._submissionService.getSubmission(this.quizId).pipe(takeUntil(this.$unsubscriver),
        map(res => res.data)).subscribe(
          data => {
            this.submission = data
          }
        );
    }
  }

  nextQuestion() {
    if (!this.questions) return;
    const question = this.questions.find(x => x.id == this.currentQuestionId);
    if (question) {
      this.currentQuestionId = this.questions[this.questions.indexOf(question) + 1].id;
      this.loadQuestion();
    }
  }

  previousQuestion() {
    if (!this.questions) return;
    const question = this.questions.find(x => x.id == this.currentQuestionId);
    if (question) {
      this.currentQuestionId = this.questions[this.questions.indexOf(question) - 1].id;
      this.loadQuestion();
    }
  }

  checkExistAnswer(answerId: number) {
    return this.submission?.answers.find(answer => answer.id === answerId) ? true : false;
  }

  checkExistQuestion(questionId: number) {
    return this.submission?.answers.find(answer => answer.questionId === questionId) ? true : false;
  }

  checkLastQuestion() {
    if (this.questions)
      return this.currentQuestionId === this.questions[this.questions?.length - 1].id;
    return false;
  }

  checkFirstQuestion() {
    if (this.questions)
      return this.currentQuestionId === this.questions[0].id;
    return false;
  }

  chooseQuestion(questionId: number) {
    this.currentQuestionId = questionId;
    this.loadQuestion();
  }

  ngOnDestroy(): void {
    this.$unsubscriver.next();
    this.$unsubscriver.complete();
  }

}
