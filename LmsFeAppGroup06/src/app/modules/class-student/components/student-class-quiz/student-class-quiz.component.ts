import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { ClassService } from '../../services/class.service';
import { QuizService } from '../../services/quiz.service';

@Component({
  selector: 'app-student-class-quiz',
  templateUrl: './student-class-quiz.component.html',
  styleUrls: ['./student-class-quiz.component.scss']
})
export class StudentClassQuizComponent implements OnInit {
  private $unsubscriber = new Subject();
  $quizs?: Observable<QuizReadDto[]>

  constructor(
    private _quizService: QuizService,
    private _classService: ClassService
  ) { }

  ngOnInit(): void {
    this.loadQuizs()
  }

  loadQuizs() {
    if (this._classService.classId)
      this.$quizs = this._quizService.getQuizs(this._classService.classId).pipe(
        takeUntil(this.$unsubscriber), map(res => res.data), tap(res => {
        },
          res => {
          }
        ));
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
