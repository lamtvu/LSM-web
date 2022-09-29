import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Result } from 'postcss';
import { Observable, Subject } from 'rxjs';
import { map, take, takeUntil, tap } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { ExerciseReadDto } from '../../../../Dto/ExerciseDto';
import { ClassService } from '../../services/class.service';
import { ExerciseService } from '../../services/exercise.service';
import { CreateExerciseDialogComponent } from '../create-exercise-dialog/create-exercise-dialog.component';

@Component({
  selector: 'app-teacher-class-exercise',
  templateUrl: './teacher-class-exercise.component.html',
  styleUrls: ['./teacher-class-exercise.component.scss']
})
export class TeacherClassExerciseComponent implements OnInit {
  private $unsubcriber = new Subject()
  private classId?: number;
  isLoading: boolean = false;

  $exercises?: Observable<ExerciseReadDto[]>

  constructor(
    private _matDialogService: MatDialog,
    private _classService: ClassService,
    private _exerviseService: ExerciseService
  ) { }

  ngOnInit(): void {
    this.classId = this._classService.classId;
    this.loadExercise();
  }

  openCreateDialog() {
    const createDialog = this._matDialogService.open(CreateExerciseDialogComponent, {
      width: '30%',
    });
    createDialog.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(
      result => {
        if (result)
          this.loadExercise();
      }
    )
  }

  openEditDialog(exercise: ExerciseReadDto) {
    const editDialog = this._matDialogService.open(CreateExerciseDialogComponent, {
      width: '30%',
      data: {...exercise}
    })
    editDialog.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(
      result => {
        if (result)
          this.loadExercise();
      }
    )
  }

  loadExercise() {
    this.isLoading = true
    if (this.classId)
      this.$exercises = this._exerviseService
        .getExercises(this.classId).pipe(map(res => res.data), tap(res => this.isLoading = false));
  }

  openDeleteDialog(exercise: ExerciseReadDto) {
    const deleteDilog = this._matDialogService.open(DeleteDialogComponent, {
      width: '30%',
      data: { title: 'Delete Exercise', deleteName: exercise.name }
    })

    deleteDilog.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(
      result => {
        if (result) {
          this.isLoading = true;
          this._exerviseService.deleteExercise(exercise.id).pipe(takeUntil(this.$unsubcriber))
            .subscribe(
              res => {
                this.loadExercise();
              }, res => {
                this.isLoading = true;
              }
            )
        }
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }

}
