import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SubmissionExerciseReadDto } from '../../../../Dto/submissionDto';
import { SubmissionExerciseService } from '../../services/submission-exercise.service';

@Component({
  selector: 'app-submission-exercise-detail-dialog',
  templateUrl: './submission-exercise-detail-dialog.component.html',
  styleUrls: ['./submission-exercise-detail-dialog.component.scss']
})
export class SubmissionExerciseDetailDialogComponent implements OnInit {
  private $unsubscriber = new Subject();
  isLoading: boolean = false;
  changeForm = this._formBuild.group({
    core: [this.submission.core, [Validators.required, Validators.pattern('^[0-9]|10{$')]],
    comment: [this.submission.comment]
  })

  errorScoreHandling(errorName: string) {
    return this.changeForm.controls['core'].hasError(errorName);
  }

  constructor(
    private _formBuild: FormBuilder,
    private _submissionService: SubmissionExerciseService,
    private _dialogRef: MatDialogRef<SubmissionExerciseReadDto>,
    @Inject(MAT_DIALOG_DATA) public submission: SubmissionExerciseReadDto
  ) { }

  ngOnInit(): void {
  }

  downloadHandling() {
    this._submissionService.dowloadFile(this.submission.id).pipe(
      takeUntil(this.$unsubscriber)).subscribe(
        res => {
          let dataType = res.type;
          let binaryData = [];
          binaryData.push(res);
          let downloadLink = document.createElement('a');
          downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
          downloadLink.setAttribute('download', this.submission.fileType);
          document.body.appendChild(downloadLink);
          downloadLink.click();
        }
      )
  }

  saveHandling() {
    if (!this.changeForm.valid) return;
    this.isLoading = true;
    this._submissionService.changeScore(this.submission.id, this.changeForm.value).pipe(
      takeUntil(this.$unsubscriber)).subscribe(
        res => {
          this.isLoading = false;
          this._dialogRef.close(true);
        },
        res => {
          this.isLoading = false;
          console.log(res);
        }
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
