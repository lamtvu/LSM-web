import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { SubmissionExerciseReadDto, SubmissionQuizReaDto } from '../../../../../Dto/submissionDto';
import { ClassService } from '../../../services/class.service';
import { SubmisstionExerciseService } from '../../../services/submisstion-exercise.service';
import { SubmisstionQuizService } from '../../../services/submisstion-quiz.service';

@Component({
  selector: 'app-student-score-list',
  templateUrl: './student-score-list.component.html',
  styleUrls: ['./student-score-list.component.scss']
})
export class StudentScoreListComponent implements OnInit {
  private $unsubscriber = new Subject();
  $submissionExercise?: Observable<SubmissionExerciseReadDto[]>;
  $submissionQuizzes?: Observable<SubmissionQuizReaDto[]>;

  isLoading: boolean = false;
  option: 'QUIZ' | 'EXERCISE' = 'EXERCISE';

  constructor(
    private _classService: ClassService,
    private _submissionQuizService: SubmisstionQuizService,
    private _submissionExerciseService: SubmisstionExerciseService
  ) { }

  ngOnInit(): void {
    this.onSelectChange();
  }

  onSelectChange() {
    if (this.option === 'QUIZ') {
      this.loadSubmissionQuiz();
      return;
    }
    this.loadSubmissionExercises();
  }

  loadSubmissionQuiz() {
    if (!this._classService.classId) return;
    this.isLoading = true;
    this.$submissionQuizzes = this._submissionQuizService.getOwnedSubmissions(this._classService.classId)
      .pipe(map(res => res.data), takeUntil(this.$unsubscriber), tap(
        data => {
          this.isLoading = false;
          console.log(data);
        }, res => {
          this.isLoading = false;
          console.log(res.error);
        }
      ))
  }


  loadSubmissionExercises() {
    if (!this._classService.classId) return;
    this.$submissionExercise = this._submissionExerciseService.getOwnedInclass(this._classService.classId)
      .pipe(map(res => res.data), takeUntil(this.$unsubscriber), tap(
        data => {
          this.isLoading = false;
          console.log(data);
        }, res => {
          console.log(res.error);
        }
      ))
  }

}
