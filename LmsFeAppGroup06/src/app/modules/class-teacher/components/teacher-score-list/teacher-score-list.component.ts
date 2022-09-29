import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { ExerciseReadDto } from '../../../../Dto/ExerciseDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { SubmissionExerciseReadDto, SubmissionQuizReaDto } from '../../../../Dto/submissionDto';
import { ClassService } from '../../services/class.service';
import { ExerciseService } from '../../services/exercise.service';
import { QuizService } from '../../services/quiz.service';
import { SubmissionExerciseService } from '../../services/submission-exercise.service';
import { SubmissionQuizService } from '../../services/submission-quiz.service';
import { SubmissionExerciseDetailDialogComponent } from '../submission-exercise-detail-dialog/submission-exercise-detail-dialog.component';

@Component({
  selector: 'app-teacher-score-list',
  templateUrl: './teacher-score-list.component.html',
  styleUrls: ['./teacher-score-list.component.scss']
})
export class TeacherScoreListComponent implements OnInit {
  private $unsubscriber = new Subject();
  $submissionExercise?: Observable<PageDataDto<SubmissionExerciseReadDto[]>>;
  $exercises?: Observable<ExerciseReadDto[]>
  $submissionQuizs?: Observable<PageDataDto<SubmissionQuizReaDto[]>>;
  $quizs?: Observable<QuizReadDto[]>;

  pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  searchValue: string = '';
  @ViewChild('paginator') paginator!: MatPaginator;

  option: 'QUIZ' | 'EXERCISE' = 'EXERCISE';

  isLoadingList: boolean = false;
  isLoadingInput: boolean = false;

  exerciseId?: number;
  constructor(
    private _exerciseService: ExerciseService,
    private _submissionExerciseService: SubmissionExerciseService,
    private _submissionQuizService: SubmissionQuizService,
    private _quizService: QuizService,
    private _dialalog: MatDialog,
    private _classService: ClassService
  ) { }

  ngOnInit(): void {
    this.loadExercise();
  }

  onSelectOptionChange() {
    if (this.option === 'QUIZ') {
      this.loadQuiz();
      return;
    }
    this.loadExercise();
  }

  loadQuiz() {
    if (!this._classService.classId) return;
    this.isLoadingInput = true;
    this.$quizs = this._quizService.getQuizs(this._classService.classId).pipe(takeUntil(this.$unsubscriber)
      , map(res => res.data), tap(res => {
        this.isLoadingInput = false
      }, res => {
        this.isLoadingInput = false
      }))
  }

  loadExercise() {
    if (!this._classService.classId) return;
    this.isLoadingInput = true;
    this.$exercises = this._exerciseService.getExercises(this._classService.classId).pipe(
      takeUntil(this.$unsubscriber), map(res => res.data), tap(
        data => {
          this.isLoadingInput = false;
        }, data => {
          this.isLoadingInput = false;
        }
      ))
  }

  onSelectExerciseChange(exerciseId: number) {
    this.isLoadingList = true;
    this.$submissionExercise = this._submissionExerciseService
      .getSubmissionByExerciseId(exerciseId, this.pageEvent.pageIndex, this.pageEvent.pageSize, this.searchValue)
      .pipe(map(res => res.data), tap(data => {
        this.exerciseId = exerciseId;
        this.isLoadingList = false;
        this.paginator.length = data.count;
      }, res => {
        this.isLoadingList = false;
        console.log(res.error);
      }));
  }

  onSelectQuizChange(quizId: number) {
    this.isLoadingList = true;
    this.$submissionQuizs = this._submissionQuizService
      .getSubmissionQuizByQuizId(quizId, this.pageEvent.pageIndex, this.pageEvent.pageSize, this.searchValue)
      .pipe(map(res => res.data), tap(data => {
        this.isLoadingList = false;
        this.paginator.length = data.count;
      }, res => {
        this.isLoadingList = false
        console.log(res.error);
      }));
  }

  openSubmissionExerciseDetail(submission: SubmissionExerciseReadDto) {
    const dialog = this._dialalog.open(SubmissionExerciseDetailDialogComponent, {
      data: submission,
      width: '30%',
      disableClose: true,
    })
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber))
      .subscribe(data => {
        console.log(this.exerciseId)
        if (data && this.exerciseId) {
          this.isLoadingList = true;
          this.$submissionExercise = this._submissionExerciseService
            .getSubmissionByExerciseId(this.exerciseId, this.pageEvent.pageIndex, this.pageEvent.pageSize, this.searchValue)
            .pipe(map(res => res.data), tap(data => {
              this.isLoadingList = false;
              this.paginator.length = data.count;
            }, res => {
              this.isLoadingList = false
              console.log(res.error);
            }));

        }
      })
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
