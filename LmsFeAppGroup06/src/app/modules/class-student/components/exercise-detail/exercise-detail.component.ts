import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, take, takeUntil, tap } from 'rxjs/operators';
import { ExerciseReadDto } from '../../../../Dto/ExerciseDto';
import { SubmissionExerciseReadDto } from '../../../../Dto/submissionDto';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { ExerciseService } from '../../services/exercise.service';
import { SubmisstionExerciseService } from '../../services/submisstion-exercise.service';

@Component({
  selector: 'app-exercise-detail',
  templateUrl: './exercise-detail.component.html',
  styleUrls: ['./exercise-detail.component.scss']
})
export class ExerciseDetailComponent implements OnInit {
  private $unsubscriber = new Subject();
  $exercise?: Observable<ExerciseReadDto>;
  $submission?: Observable<SubmissionExerciseReadDto>;
  exerciseId?: number;
  isLoading = false;
  isUploading = false;
  isRemoving = false;
  uploadFile?: File | null;

  constructor(
    private _activateRouter: ActivatedRoute,
    private _exerciseService: ExerciseService,
    private _submissionService: SubmisstionExerciseService,
    private _dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this._activateRouter.params.pipe(takeUntil(this.$unsubscriber))
      .subscribe(data => {
        this.exerciseId = data.id;
        this.loadExercise();
        this.loadSubmissionExercise();
      })
  }

  handleFileInput(event: any) {
    if (event.target.files.length > 0)
      this.uploadFile = event.target.files[0];
  }

  loadExercise() {
    if (!this.exerciseId) return;
    this.isLoading = true;
    this.$exercise = this._exerciseService.getExercise(this.exerciseId).pipe(takeUntil(this.$unsubscriber),
      map(res => res.data),
      tap(res => this.isLoading = false,
        res => this.isLoading = false));
  }

  loadSubmissionExercise() {
    if (!this.exerciseId) return;
    this.isLoading = true;
    this.$submission = this._submissionService.getSubmisstion(this.exerciseId).pipe(
      takeUntil(this.$unsubscriber), map(res => res.data), tap(data => {
        this.isLoading = false;
        console.log(data);
      }, data => this.isLoading = false)
    )
  }

  submitHandling() {
    if (!this.exerciseId || !this.uploadFile) return;
    const formData = new FormData();
    formData.append('file', this.uploadFile);
    this.isUploading = true;
    this._submissionService.upLoadSubmisstion(this.exerciseId, formData).pipe(
      takeUntil(this.$unsubscriber)).subscribe(
        res => {
          this.isUploading = false;
          this.loadSubmissionExercise();
        },
        res => {
          this.isUploading = false;
        }
      )
  }

  deleteSubmit(submisstion: SubmissionExerciseReadDto) {
    const dialog = this._dialog.open(DeleteDialogComponent,
      { data: { title: 'Delete Submisstion', deleteName: submisstion.fileType } });
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      data => {
        if (data) {
          this.isRemoving = true;
          this._submissionService.deleteSubmission(submisstion)
            .pipe(takeUntil(this.$unsubscriber)).subscribe(
              res => {
                this.loadSubmissionExercise();
                this.isRemoving = false;
              }, res => {
                this.isRemoving = false;
              }
            )
        }
      }
    )
  }

  downloadHandling(submission: SubmissionExerciseReadDto) {
    this._submissionService.dowloadFile(submission.id).pipe(
      takeUntil(this.$unsubscriber)).subscribe(
        res => {
          let dataType = res.type;
          console.log(res.type)
          let binaryData = [];
          binaryData.push(res);
          let downloadLink = document.createElement('a');
          downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
          downloadLink.setAttribute('download', submission.fileType);
          document.body.appendChild(downloadLink);
          downloadLink.click();
        }
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
