import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { ExerciseReadDto } from '../../../../Dto/ExerciseDto';
import { ClassService } from '../../services/class.service';
import { ExerciseService } from '../../services/exercise.service';

@Component({
  selector: 'app-student-class-exercise',
  templateUrl: './student-class-exercise.component.html',
  styleUrls: ['./student-class-exercise.component.scss']
})
export class StudentClassExerciseComponent implements OnInit {
  private $unsubscriber = new Subject();
  $exercise?: Observable<ExerciseReadDto[]>;
  isLoading: boolean = false;

  constructor(
    private _classService: ClassService,
    private _exerciesService: ExerciseService
  ) { }

  ngOnInit(): void {
    this.loadExercise();
  }

  loadExercise() {
    if (!this._classService.classId) return;
    this.isLoading = true;
    this.$exercise = this._exerciesService.getExercises(this._classService.classId).pipe(
      takeUntil(this.$unsubscriber), map(res => res.data), tap(data => {
        this.isLoading = false;
      }, data => {
        this.isLoading = false;
      })
    );
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
