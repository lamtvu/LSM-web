import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { QuizService } from 'src/app/modules/class-teacher/services/quiz.service';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { QuizReadDto } from '../../../../Dto/QuizDto';
import { ClassService } from '../../services/class.service';
import { CreateQuizDialogComponent } from '../create-quiz-dialog/create-quiz-dialog.component';

@Component({
  selector: 'app-teacher-class-quiz',
  templateUrl: './teacher-class-quiz.component.html',
  styleUrls: ['./teacher-class-quiz.component.scss']
})
export class TeacherClassQuizComponent implements OnInit {
  private $unsubscriber = new Subject();
  private classId?: number;
  isLoading: boolean = false;
  $quizs?: Observable<QuizReadDto[]>;
  constructor(
    private _matDialogService: MatDialog,
    private _quizService: QuizService,
    private _classService: ClassService,
  ) { }

  ngOnInit(): void {
    this.classId = this._classService.classId;
    this._classService.classIdEmit.pipe(takeUntil(this.$unsubscriber))
      .subscribe(id => {
        this.classId = id;
        this.loadQuiz();
      });
    this.loadQuiz();
  }

  openCreateDialog() {
    const createDialog = this._matDialogService.open(CreateQuizDialogComponent, {
      width: '30%'
    });

    createDialog.beforeClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      result => {
        if (result)
          this.loadQuiz();
      }
    )
  }

  openEditDialog(quiz: QuizReadDto) {
    const editDialog = this._matDialogService.open(CreateQuizDialogComponent, {
      width: '30%',
      data: { ...quiz }
    });

    editDialog.beforeClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      result => {
        console.log(result)
        if (result)
          this.loadQuiz();
      }
    )
  }
  
  openDeleteDialog(quiz: QuizReadDto) {
    const deleteDialog = this._matDialogService
      .open(DeleteDialogComponent, {
        data: {
          title: 'Delete Quiz', deleteName: quiz.name
        }
      });

    deleteDialog.afterClosed()
      .pipe(takeUntil(this.$unsubscriber)).subscribe(
        result => {
          if (result) {
            this.isLoading = true;
            this._quizService.deleteQuiz(quiz.id)
              .pipe(takeUntil(this.$unsubscriber)).subscribe(
                res => {
                  this.loadQuiz()
                }, res => this.isLoading = false
              )
          }
        }
      );


  }

  loadQuiz() {
    if (!this.classId) return;
    this.isLoading = true;
    this.$quizs = this._quizService
      .getQuizs(this.classId)
      .pipe(
        map(res => res.data),
        tap(res => this.isLoading = false)
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
