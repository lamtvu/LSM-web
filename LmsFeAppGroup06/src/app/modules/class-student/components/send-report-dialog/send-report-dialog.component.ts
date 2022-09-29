
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ReportReadDTO } from '../../../../Dto/report-read-dto';
import { ClassService } from 'src/app/modules/class-student/services/class.service';
import { ReportService } from 'src/app/modules/class-student/services/report.service';

@Component({
  selector: 'app-send-report-dialog',
  templateUrl: './send-report-dialog.component.html',
  styleUrls: ['./send-report-dialog.component.scss']
})
export class SendReportDialogComponent implements OnInit {

  private $unsubsriber = new Subject();

  reportForm = this._formBuilder.group({
    'title': ['', [
      Validators.required,
    ]],
    'content': ['', [
      Validators.required,
    ]]
  })

  constructor(
    @Inject(MAT_DIALOG_DATA) public report: ReportReadDTO,
    private _dialogRef: MatDialogRef<SendReportDialogComponent>,
    private _formBuilder: FormBuilder,
    private _reportService: ReportService,
    private _classService: ClassService
  ) { }

 ngOnInit(): void {
   
  }

  clickHandling() {
    
    this.createHandling();
  }

  createHandling() {
    console.log(this.reportForm.value)
    if (!this.reportForm.valid)
      return;
    if (this._classService.classId)
      this._reportService.createReport(this._classService.classId,
        this.reportForm.value).pipe(takeUntil(this.$unsubsriber))
        .subscribe(
          res => {
            console.log(res.data);
            this._dialogRef.close(true);
          },
          res => {
            console.log(res);
          }
        )
  }
  errorHandling(controlName: string, errorName: string) {
    return this.reportForm.controls[controlName].hasError(errorName);
  }

  ngOnDestroy(): void {
    this.$unsubsriber.next();
    this.$unsubsriber.complete();
  }
}
